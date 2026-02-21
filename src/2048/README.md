# 2048

Physics-based версия 2048 на Unity. Проект построен на `Unity + C# + Zenject + UGUI/TextMeshPro`, и этот README предназначен для быстрого онбординга внутренней команды.

## Что за проект
- Жанр: arcade/puzzle с физикой.
- Основная механика: спавн кубов, merge одинаковых значений, рост счёта, условия победы/поражения.
- Архитектурный стиль: state machine + DI (Zenject) + сервисный слой.

## Как запустить
### Предусловия
- Unity Editor `6000.3.8f1`.
- Доступ к репозиторию и пакетам проекта.

### Быстрый запуск
1. Откройте проект через Unity Hub.
2. Дождитесь полного импорта ассетов и пакетов.
3. Проверьте Build Settings: в списке сцен должны быть `Initial`, `Empty`, `Main`.
4. Откройте `Assets/Scenes/Initial.unity` и запустите Play Mode.

### Быстрый runtime-check
1. Куб появляется по input.
2. Merge одинаковых кубов работает.
3. Счёт обновляется в UI.
4. Окна `Lose`/`Victory` показываются по условиям.

## Где код
### Карта проекта
- `Assets/Scripts/Infrastructure` — boot, state machine, DI, scene loading, фабрики.
- `Assets/Scripts/Gameplay` — кубы, input handler, spawner, game over point.
- `Assets/Scripts/Services` — merge/game over/random/pool/providers.
- `Assets/Scripts/UI` — HUD, окна, window service/factory.
- `Assets/Scripts/Data` — `WorldData`, счёт и события.
- `Assets/Resources` — runtime prefab/assets (`Resources.Load`).
- `ProjectSettings` — конфигурация Unity, включая build scenes.

### С чего читать код
- `Assets/Scripts/Infrastructure/Installers/BootstrapInstaller.cs`
- `Assets/Scripts/Infrastructure/States/BootstrapState.cs`
- `Assets/Scripts/Infrastructure/States/LoadLevelState.cs`
- `Assets/Scripts/Infrastructure/Factory/Game/GameFactory.cs`
- `Assets/Scripts/Services/Merge/MergeService.cs`
- `Assets/Scripts/Services/GameOver/GameOverService.cs`
- `Assets/Scripts/UI/Services/Windows/WindowService.cs`

## Документация
- [AGENTS.md](AGENTS.md) — правила внесения изменений и рабочие ограничения.
- [ARCHITECTURE.md](ARCHITECTURE.md) — подробная архитектура, диаграммы, инварианты.

## Принципы этого README
- Короткий формат для быстрого входа.
- Только `repo-relative` пути.
- Без дублирования детальной архитектуры (она в `ARCHITECTURE.md`).
