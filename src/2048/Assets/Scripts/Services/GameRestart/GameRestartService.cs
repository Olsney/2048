using Data;
using Infrastructure.States;
using UnityEngine;

namespace Services.GameRestart
{
    public class GameRestartService : IGameRestartService
    {
        private const string MainSceneName = "Main";

        private readonly GameStateMachine _stateMachine;
        private readonly IWorldData _worldData;

        public GameRestartService(GameStateMachine stateMachine, IWorldData worldData)
        {
            _stateMachine = stateMachine;
            _worldData = worldData;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            _worldData.Reset();
            _stateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }
    }
}
