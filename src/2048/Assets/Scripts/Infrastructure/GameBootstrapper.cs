using Infrastructure.Factory.Game;
using Infrastructure.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameStateMachine _gameStateMachine;
        
        [Inject]
        private void Construct(GameStateMachine gameStateMachine, IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        private void Start()
        {
            _gameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}