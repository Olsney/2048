using Gameplay.Cubes;
using UI.Services.Windows;
using UnityEngine;

namespace Services.GameOver
{
    public class GameOverService : IGameOverService
    {
        private readonly IWindowService _windowService;
        private const int VictoryCubeValue = 131072;

        public GameOverService(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void TryFinish(Cube cube)
        {
            if (cube == null)
                return;

            if (cube.Value >= VictoryCubeValue)
            {
                _windowService.Open(WindowType.VictoryWindow);
                FinishGame();
                return;
            }
            
            if (cube.HasEnteredPlayArea)
            {
                _windowService.Open(WindowType.DefeatWindow);
                FinishGame();
            }
        }

        private static void FinishGame() => 
            Time.timeScale = 0;
    }
}
