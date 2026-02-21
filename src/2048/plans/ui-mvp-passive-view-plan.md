# План миграции UI на MVP Passive View (strict)

## Краткое резюме
1. Цель: перевести весь UI-слой (HUD, окна, элементы UI в мире) на `MVP Passive View`, чтобы `View` не зависели от `IWorldData` и не содержали реакций на доменные события.
2. Зафиксированные решения: охват `UI + окна + HUD`, строгий Passive View, `1 Presenter = 1 View`, включаем naming cleanup, план-файл: `plans/ui-mvp-passive-view-plan.md`.
3. Ограничение текущего режима: в Plan Mode файл не создаётся; при старте выполнения первым шагом создаётся этот `.md` и туда записывается согласованный план.

## Scope
1. Включено:
- `Assets/Scripts/UI/**`
- прямые точки интеграции UI в `Assets/Scripts/Infrastructure/Factory` и `Assets/Scripts/Infrastructure/States`
- UI-часть именования (`ActorUI`, `LoseWindow` как класс UI), без изменения сцен.
2. Не включено:
- gameplay-сервисы (`MergeService`, `GameOverService`) кроме минимальных compile-safe правок ссылок на новые UI-типы
- сторонние плагины
- массовые нефункциональные рефакторы вне UI-границы.

## Целевое состояние архитектуры MVP
1. `View`:
- только отображение и UI-сигналы
- без зависимостей от `IWorldData` и доменных сервисов.
2. `Presenter`:
- получает доменные данные через DI
- подписывается/отписывается на события модели
- обновляет `View` через интерфейс.
3. Связь:
- `View` -> события UI (`CloseRequested` и т.д.)
- `Presenter` -> команды отображения (`SetScore`, `SetMessage` и т.д.).

## Изменения по модулям (decision-complete)

## 1) План-файл
1. Создать `plans/ui-mvp-passive-view-plan.md`.
2. Записать в него этот план как источник выполнения.

## 2) HUD (бывший ActorUI)
1. Переименовать `ActorUI` в `ScoreHudPresenter`.
2. Добавить интерфейс `IScoreHudView` с методом `SetScore(int score)`.
3. Сделать `ScoreView` реализацией `IScoreHudView`.
4. Логика подписки на `IWorldData.Changed` остаётся только в `ScoreHudPresenter`.
5. Убрать подписку на `IWorldData` из любых View-компонентов HUD.
6. Обновить prefab HUD на новый компонент presenter и ссылки на view.

## 3) Окна Victory/Defeat
1. Разделить текущие `VictoryWindow`/`LoseWindow` на View + Presenter.
2. Добавить интерфейсы view:
- `IVictoryWindowView`
- `IDefeatWindowView`
3. `WindowBase` сделать базой только для UI-поведения (без `IWorldData`).
4. Перенести подписки `WorldData.Changed` из view в presenter:
- `VictoryWindowPresenter`
- `DefeatWindowPresenter`
5. Naming cleanup:
- `LoseWindow` (класс UI) -> `DefeatWindowView`.
6. Обновить префабы окон на новые компоненты presenter/view.

## 4) Cube world UI (CubeValue)
1. Убрать подписку на `Cube.ValueUpdated` из `CubeValueView`.
2. Добавить `CubeValuePresenter` (на объекте куба рядом с view), который:
- подписывается на `Cube.ValueUpdated`
- передаёт значение в view через интерфейс `ICubeValueView`.
3. `CubeValueView` оставить только с методами визуализации текста.

## 5) UIFactory/WindowService интеграция
1. Сохранить существующие публичные контракты:
- `IUIFactory`
- `IWindowService`
2. Обновить внутренние привязки на новые UI-классы и префабы после rename.
3. Проверить, что `WindowType.DefeatWindow` открывает новый Defeat View/Presenter набор.

## 6) DI и lifecycle
1. Presenter-компоненты делают subscribe в `OnEnable`/`Start` и unsubscribe в `OnDisable`/`OnDestroy`.
2. Проверить, что Zenject-инъекции для presenter/view компонентов корректны на префабах.
3. Не добавлять глобальные статики.

## Важные изменения API/типов
1. Новые интерфейсы:
- `IScoreHudView`
- `IVictoryWindowView`
- `IDefeatWindowView`
- `ICubeValueView`
2. Новые presenter-классы:
- `ScoreHudPresenter`
- `VictoryWindowPresenter`
- `DefeatWindowPresenter`
- `CubeValuePresenter`
3. Naming cleanup UI-классов:
- `ActorUI` -> `ScoreHudPresenter`
- `LoseWindow` (UI class) -> `DefeatWindowView`
4. Публичные контракты `IUIFactory` и `IWindowService` сохраняются без расширения (breaking changes не планируются).

## Тесты и сценарии проверки

## 1) Автотесты
1. По текущему ограничению задачи автотесты не добавляются.

## 2) Smoke checklist (ручной)
1. Сцены: `Initial -> Empty -> Main`.
2. Spawn/merge: счёт растёт, HUD обновляется.
3. Victory: открывается victory окно, текст корректный, без повторных подписок.
4. Defeat: открывается defeat окно, текст корректный, без повторных подписок.
5. При destroy окон/объектов нет утечек подписок.
6. `StaticDataService` корректно резолвит UI префабы.
