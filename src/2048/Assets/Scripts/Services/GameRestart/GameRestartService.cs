using Data;
using Infrastructure.States;
using Services.CubePools;
using UnityEngine;

namespace Services.GameRestart
{
    public class GameRestartService : IGameRestartService
    {
        private const string MainSceneName = "Main";

        private readonly GameStateMachine _stateMachine;
        private readonly IWorldData _worldData;
        private readonly ICubePool _cubePool;

        public GameRestartService(GameStateMachine stateMachine, IWorldData worldData, ICubePool cubePool)
        {
            _stateMachine = stateMachine;
            _worldData = worldData;
            _cubePool = cubePool;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            _cubePool.Clear();
            _worldData.Reset();
            _stateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }
    }
}
