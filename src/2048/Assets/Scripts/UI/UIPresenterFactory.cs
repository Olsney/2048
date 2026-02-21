using Data;
using Gameplay.Cubes;
using Services.GameRestart;
using UI.Elements;
using UI.Windows;

namespace UI
{
    public class UIPresenterFactory : IUIPresenterFactory
    {
        private readonly IWorldData _worldData;
        private readonly IGameRestartService _gameRestartService;

        public UIPresenterFactory(IWorldData worldData, IGameRestartService gameRestartService)
        {
            _worldData = worldData;
            _gameRestartService = gameRestartService;
        }

        public ScoreHudPresenter CreateScoreHudPresenter(IScoreHudView view) =>
            new(view, _worldData);

        public VictoryWindowPresenter CreateVictoryWindowPresenter(IVictoryWindowView view) =>
            new(view, _worldData, _gameRestartService);

        public DefeatWindowPresenter CreateDefeatWindowPresenter(IDefeatWindowView view) =>
            new(view, _worldData, _gameRestartService);

        public CubeValuePresenter CreateCubeValuePresenter(Cube cube, ICubeValueView view) =>
            new(view, cube);
    }
}
