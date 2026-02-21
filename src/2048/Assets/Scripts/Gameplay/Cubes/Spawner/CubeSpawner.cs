using System;
using Gameplay.Input;
using Infrastructure.Factory.Game;
using Services.InputHandlerProviders;
using Services.Randoms;
using UnityEngine;
using Zenject;

namespace Gameplay.Cubes.Spawner
{
    public class CubeSpawner : MonoBehaviour
    {
        private const float ForceModifier = 3f;
        private IPlayerInputHandlerProvider _playerInputHandlerProvider;
        private IGameFactory _gameFactory;
        private IPlayerInputEvents _playerInput;
        private IRandomService _randomService;

        [Inject]
        public void Construct(IGameFactory gameFactory,
            IPlayerInputHandlerProvider playerInputHandlerProvider,
            IRandomService randomService)
        {
            _gameFactory = gameFactory;
            _playerInputHandlerProvider = playerInputHandlerProvider;
            _randomService = randomService;
        }

        public void Initialize()
        {
            UnsubscribeFromInput();
            
            _playerInput = _playerInputHandlerProvider.Get();
            
            if (_playerInput == null)
                throw new InvalidOperationException($"{nameof(IPlayerInputEvents)} is not initialized.");
            
            _playerInput.TapEnded += SpawnRandomAtSpawnPoint;
        }

        private void UnsubscribeFromInput()
        {
            if (_playerInput == null)
                return;

            _playerInput.TapEnded -= SpawnRandomAtSpawnPoint;
            _playerInput = null;
        }

        private void OnDestroy() => 
            UnsubscribeFromInput();

        public GameObject SpawnMerge(int cubeValue, Vector3 at)
        {
            GameObject mergedCube = _gameFactory.CreateCube(cubeValue, at);
            SetColor(mergedCube, cubeValue);

            Cube cube = mergedCube.GetComponent<Cube>();
            cube.MarkAsEnteredPlayArea();

            Rigidbody rigidbody = mergedCube.GetComponent<Rigidbody>();
            AddRandomForceToNewCube(rigidbody);

            return mergedCube;
        }

        private void SpawnRandomAtSpawnPoint(Vector2 _)
        {
            int cubeValue = _randomService.GetRandomPowerOfTwoValue();

            GameObject cube = _gameFactory.CreateCube(cubeValue);

            SetColor(cube, cubeValue);
        }

        private void SetColor(GameObject cube, int value)
        {
            if (CubeColors.TryGet(value, out Color color))
            {
                if (cube.TryGetComponent(out Renderer renderer))
                    renderer.material.color = color;
            }
        }

        private void AddRandomForceToNewCube(Rigidbody rigidbody)
        {
            float min = -1f;
            float max = 1f;

            Vector3 randomDirection = Vector3.up + new Vector3(
                _randomService.Next(min, max),
                _randomService.Next(min, max),
                _randomService.Next(min, max)
            ).normalized;

            rigidbody.AddForce(randomDirection * ForceModifier, ForceMode.Impulse);
        }
    }
}
