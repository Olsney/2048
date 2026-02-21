using Data;
using Gameplay.Cubes;
using UI.Elements;
using UI.Windows;

namespace UI
{
    public class UIPresenterFactory : IUIPresenterFactory
    {
        private readonly IWorldData _worldData;

        public UIPresenterFactory(IWorldData worldData)
        {
            _worldData = worldData;
        }

        public ScoreHudPresenter CreateScoreHudPresenter(IScoreHudView view) =>
            new(view, _worldData);

        public VictoryWindowPresenter CreateVictoryWindowPresenter(IVictoryWindowView view) =>
            new(view, _worldData);

        public DefeatWindowPresenter CreateDefeatWindowPresenter(IDefeatWindowView view) =>
            new(view, _worldData);

        public CubeValuePresenter CreateCubeValuePresenter(Cube cube, ICubeValueView view) =>
            new(view, cube);
    }
}
