using Gameplay.Cubes;
using UI.Elements;
using UI.Windows;

namespace UI
{
    public interface IUIPresenterFactory
    {
        ScoreHudPresenter CreateScoreHudPresenter(IScoreHudView view);
        VictoryWindowPresenter CreateVictoryWindowPresenter(IVictoryWindowView view);
        DefeatWindowPresenter CreateDefeatWindowPresenter(IDefeatWindowView view);
        CubeValuePresenter CreateCubeValuePresenter(Cube cube, ICubeValueView view);
    }
}
