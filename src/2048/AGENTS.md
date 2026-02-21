# AGENTS.md

## Цель документа
Этот файл задаёт правила для любых AI-агентов и разработчиков, вносящих изменения в проект `2048` (Unity).
Приоритет: сохранить рабочий игровой цикл, DI-конфигурацию Zenject и совместимость со сценами/ресурсами.

## Навигация по документации
- `AGENTS.md` фиксирует operational-правила работы и изменения кода.
- `ARCHITECTURE.md` фиксирует устройство системы, runtime-flow, контракты и инварианты.
- При архитектурных изменениях ориентироваться на `ARCHITECTURE.md` и держать оба документа синхронизированными.

## Политика путей в документах
- Использовать только `repo-relative` пути (например: `Assets/Code/Gameplay`, `ProjectSettings/EditorBuildSettings.asset`).
- Не использовать абсолютные пути в проектной документации.

## Технологический контекст
- Движок: Unity `6000.3.8f1` (`ProjectSettings/ProjectVersion.txt`).
- Язык: C#.
- DI: Zenject (`Assets/Plugins/Zenject`).
- UI: UGUI + TextMeshPro + DOTween.
- Поддерживаемые сцены: `Initial`, `Empty`, `Main` (см. `ProjectSettings/EditorBuildSettings.asset`).

## Карта проекта
- Игровая логика: `Assets/Code/Gameplay`
- Инфраструктура/State machine/загрузка сцен: `Assets/Code/Infrastructure`
- Сервисы: `Assets/Code/Services`
- UI: `Assets/Code/UI`
- Данные мира: `Assets/Code/Data`
- Ресурсы (prefab/asset и static data): `Assets/Resources`

## Критический runtime-поток
1. `GameBootstrapper` запускает `GameStateMachine`.
2. `BootstrapState` гарантирует вход через сцену `Initial`.
3. `LoadProgressState` переводит в `LoadLevelState("Main")`.
4. `LoadLevelState` загружает `Empty` -> целевую сцену, затем создаёт UI и игровой мир через фабрики.
5. `GameFactory` создаёт `PlayerInputHandler`, `CubeSpawner`, кубы через `CubePool`.
6. `MergeService` объединяет кубы, начисляет очки в `WorldData`, спавнит новый куб.
7. `GameOverService` открывает окно победы/поражения и ставит `Time.timeScale = 0`.

Любые изменения, ломающие эту цепочку, считаются регрессией.

## Обязательные правила изменений
- Не редактировать сторонние библиотеки без прямой необходимости:
  - `Assets/Plugins/Zenject/**`
  - ассеты из `Assets/Packages/**`
- Не коммитить/не править вручную артефакты Unity:
  - `Library/**`, `Temp/**`, `Logs/**`, `UserSettings/**`
  - автогенерируемые `*.csproj`, `*.sln` (если задача не про них)
- Для новых runtime-сервисов обязательно:
  1. добавить интерфейс;
  2. добавить реализацию;
  3. зарегистрировать биндинг в `Assets/Code/Infrastructure/Installers/BootstrapInstaller.cs`.
- Для новых состояний обязательно:
  1. добавить класс состояния;
  2. зарегистрировать в `BootstrapInstaller.BindStates()`;
  3. убедиться, что переходы в `GameStateMachine` реально достижимы.
- Для новых prefab/окон обязательно:
  1. добавить/обновить запись:
     - для world/gameplay/ui-root prefab: `Assets/Resources/StaticData/Common/PrefabsStaticData.asset`;
     - для окон: `Assets/Resources/StaticData/Window/WindowStaticData.asset`;
  2. проверить фактический prefab в `Assets/Resources/**`;
  3. убедиться, что `StaticDataService` резолвит prefab без `null`.
- Не переименовывать сцены `Initial`, `Empty`, `Main` без синхронного обновления:
  - `BootstrapState`, `LoadProgressState`, `LoadLevelState`, `EditorBuildSettings.asset`.
- Любые подписки на события (`TapStarted`, `TapEnded`, `WorldData.Changed`) должны иметь гарантированную отписку в `Cleanup`/`OnDestroy`.

## Архитектурные соглашения
- Предпочитать DI через Zenject и интерфейсы (`I*Service`, `I*Factory`, `I*Provider`).
- Не создавать глобальные статики для игрового состояния; использовать `IWorldData` и сервисы.
- Логику объединения/скора держать в сервисах (`MergeService`, `WorldData`), а не в UI.
- UI-окна создавать через `IWindowService` + `IUIFactory`, не напрямую из геймплея.
- Пул объектов (`CubePool`) обязателен для кубов; не возвращаться к `Instantiate` в hot path.

## Чеклист перед завершением задачи
- Проект компилируется в Unity без новых ошибок C#.
- Переход сцен работает: `Initial` -> `Empty` -> `Main`.
- Спавн куба по input работает.
- Merge работает: создаётся новый куб, счёт увеличивается.
- Окна победы/поражения открываются корректно.
- Нет утечек подписок на события после уничтожения объектов.
- Конфиги `Assets/Resources/StaticData/Common/PrefabsStaticData.asset` и `Assets/Resources/StaticData/Window/WindowStaticData.asset` соответствуют реальным prefab в `Assets/Resources`.

## Чего избегать
- Вносить косметические массовые рефакторы без продуктовой необходимости.
- Менять публичные контракты интерфейсов без обновления всех внедрений и биндингов.
- Смешивать ответственность UI и Gameplay в одном классе.
- Дублировать провайдеры одиночек, если уже есть существующие (`CubeSpawnerProvider`, `PlayerInputHandlerProvider`, `CubeSpawnPointProvider`).

## Минимальный формат отчёта по изменениям
В каждом отчёте агента указывать:
1. Что изменено (файлы и кратко зачем).
2. Какие инварианты/потоки проверены.
3. Какие риски остались (если есть).
