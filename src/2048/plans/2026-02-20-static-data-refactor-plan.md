# План рефактора: переход с `AssetPath`/`[SerializeField]-values` на `StaticData`

## Краткое резюме
1. Убираем runtime-загрузку префабов через `AssetPath` и переводим её на `IStaticDataService`.
2. Переносим только числовые значения из `[SerializeField]` в static data. Ссылки на компоненты в префабах не трогаем.
3. Реализуем `StaticDataService` в стиле `Resources.Load/LoadAll` + кэш словарей + typed-getters.
4. Обновляем `AGENTS.md` и `ARCHITECTURE.md` под новую модель.

## Зафиксированные решения
1. Формат: `ScriptableObject`.
2. Скоуп: только значения `[SerializeField]` (без переноса всех констант проекта).
3. `AssetPath`: удалить полностью после миграции.
4. Структура static data: несколько SO + словари в сервисе.
5. Инициализация static data: через `Resources.Load/LoadAll` внутри `StaticDataService`.

## Изменения в коде

### 1) Static Data модели и сервис
1. Добавить enum идентификаторов префабов, например `PrefabId`.
   Файл: `Assets/Scripts/Services/StaticData/PrefabId.cs`
2. Добавить SO-конфиг для префабов (список `PrefabId -> GameObject`).
   Файл: `Assets/Scripts/Services/StaticData/Configs/PrefabStaticData.cs`
3. Добавить SO-конфиг gameplay-параметров куба.
   Файл: `Assets/Scripts/Services/StaticData/Configs/CubeGameplayStaticData.cs`
4. Добавить SO-конфиг UI-анимации счёта.
   Файл: `Assets/Scripts/Services/StaticData/Configs/ScoreViewStaticData.cs`
5. Расширить интерфейс `IStaticDataService`:
   - `void LoadAll()`
   - `GameObject GetPrefab(PrefabId id)`
   - `CubeGameplayStaticData CubeGameplay { get; }`
   - `ScoreViewStaticData ScoreView { get; }`
   Файл: `Assets/Scripts/Services/StaticData/IStaticDataService.cs`
6. Реализовать `StaticDataService`:
   - `Resources.Load<...>()` для singleton-конфигов
   - кэш словаря префабов
   - явные исключения при отсутствии данных
   - идемпотентный `LoadAll()`
   Файл: `Assets/Scripts/Services/StaticData/StaticDataService.cs`

### 2) Runtime и DI
1. В `BootstrapState` инжектировать `IStaticDataService` и вызывать `LoadAll()` до перехода в `LoadProgressState`.
   Файл: `Assets/Scripts/Infrastructure/States/BootstrapState.cs`
2. В `GameFactory`, `UIFactory`, `CubePool` заменить использование `AssetPath` на `IStaticDataService.GetPrefab(...)`.
   Файлы:
   - `Assets/Scripts/Infrastructure/Factory/Game/GameFactory.cs`
   - `Assets/Scripts/UI/Factory/UIFactory.cs`
   - `Assets/Scripts/Services/CubePools/CubePool.cs`
3. Удалить слой `IAssetProvider/AssetProvider` и биндинг из `BootstrapInstaller`, если больше нет потребителей.
   Файлы:
   - `Assets/Scripts/Infrastructure/AssetManagement/IAssetProvider.cs`
   - `Assets/Scripts/Infrastructure/AssetManagement/AssetProvider.cs`
   - `Assets/Scripts/Infrastructure/Installers/BootstrapInstaller.cs`
4. Удалить `AssetPath` и все ссылки на него.
   Файл: `Assets/Scripts/Infrastructure/AssetManagement/AssetPath.cs`

### 3) Перенос `[SerializeField]` значений в static data
1. `CubeMover`: убрать числовые `[SerializeField]`, брать значения из `IStaticDataService.CubeGameplay`.
   Файл: `Assets/Scripts/Gameplay/Cubes/CubeMover.cs`
2. `Cube`: убрать `_minMergeImpulse` из `[SerializeField]`, брать из `IStaticDataService.CubeGameplay`.
   Файл: `Assets/Scripts/Gameplay/Cubes/Cube.cs`
3. `ScoreView`: убрать `_punchScale`, `_scaleDuration`, `_countDuration` из `[SerializeField]`, брать из `IStaticDataService.ScoreView`.
   Файл: `Assets/Scripts/UI/Elements/ScoreView.cs`
4. Ссылочные поля не менять: `_counter`, `_text`, `_closeButton`, `_cube`, `_texts`, `_scoreView`.

### 4) Создание ассетов static data
1. Создать assets в `Resources/StaticData`:
   - `PrefabsStaticData.asset`
   - `CubeGameplayStaticData.asset`
   - `ScoreViewStaticData.asset`
2. Перенести значения без изменения поведения:
   - `CubeMover`: `left=-0.6`, `right=2.7`, `distance=10`, `launchForce=15`
   - `Cube`: `minMergeImpulse=0.1`
   - `ScoreView`: `punchScale=1.2`, `scaleDuration=0.2`, `countDuration=0.3`

### 5) Обновление документации
1. Обновить `AGENTS.md`:
   - убрать требование обязательного добавления `AssetPath` для новых префабов
   - добавить правило: новые runtime-параметры и prefab-ссылки идут через static data конфиги
2. Обновить `ARCHITECTURE.md`:
   - заменить раздел контракта ресурсов с `AssetPath` на `IStaticDataService` + SO-конфиги
   - обновить DI-карту и runtime-flow (загрузка static data в bootstrap)

## Изменения публичных контрактов/интерфейсов
1. `IStaticDataService` становится рабочим контрактом (`LoadAll`, getters).
2. `IAssetProvider` и `AssetPath` удаляются из runtime-контракта (после миграции потребителей).

## Тест-кейсы и сценарии приёмки
1. Boot-flow работает: `Initial -> Empty -> Main`.
2. `StaticDataService.LoadAll()` вызывается один раз в bootstrap и не падает.
3. Спавн куба по input работает.
4. Merge работает: старые кубы в pool, новый создаётся, счёт растёт.
5. UI score анимируется с теми же таймингами.
6. Окна victory/lose открываются корректно.
7. Нет утечек подписок после destroy/reuse (особенно `CubeMover` и `WorldData.Changed`).
8. В проекте нет ссылок на `AssetPath` и `IAssetProvider`.

## Риски и смягчение
1. Риск потери баланса из-за неверного переноса значений.
   Смягчение: переносить фактические значения из текущих prefab/кода.
2. Риск `null`-конфигов в рантайме.
   Смягчение: fail-fast исключения с именем отсутствующего asset.
3. Риск регресса DI.
   Смягчение: загрузка static data строго в `BootstrapState` до создания мира/UI.

## Явные допущения и выбранные дефолты
1. Константы вне `[SerializeField]` (`ForceModifier`, `MaxCubeValue`, RNG-шансы) не переносим в этой задаче.
2. Тексты окон не переносим в static data.
3. Новые config-assets размещаем в `Resources/StaticData`.
4. Подход через `Resources` сохраняем как основной (без Addressables).
