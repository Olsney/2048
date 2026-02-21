using System;
using Gameplay.Cubes;
using Gameplay.Cubes.Spawner;
using Gameplay.Input;
using Services.CubePools;
using Services.CubeSpawnerProviders;
using Services.InputHandlerProviders;
using Services.Randoms;
using Services.Scene;
using Services.SpawnPointProviders;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory.Game
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        private readonly IPlayerInputHandlerProvider _playerInputHandlerProvider;
        private readonly ICubeSpawnPointProvider _cubeSpawnPointProvider;
        private readonly IRandomService _randomService;
        private readonly ICubeSpawnerProvider _cubeSpawnerProvider;
        private readonly ICubePool _cubePool;
        private readonly ISceneProvider _sceneProvider;

        public GameFactory(IInstantiator instantiator, 
            IStaticDataService staticData,
            IPlayerInputHandlerProvider playerInputHandlerProvider,
            ICubeSpawnPointProvider cubeSpawnPointProvider,
            IRandomService randomService,
            ICubeSpawnerProvider cubeSpawnerProvider,
            ICubePool cubePool,
            ISceneProvider sceneProvider)
        {
            _instantiator = instantiator;
            _staticData = staticData;
            _playerInputHandlerProvider = playerInputHandlerProvider;
            _cubeSpawnPointProvider = cubeSpawnPointProvider;
            _randomService = randomService;
            _cubeSpawnerProvider = cubeSpawnerProvider;
            _cubePool = cubePool;
            _sceneProvider = sceneProvider;
        }
        
        public GameObject CreatePlayerInputHandler()
        {
            GameObject prefab = _staticData.GetPrefab(PrefabId.PlayerInputHandler);

            GameObject instance = _instantiator.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, SceneRoot());

            PlayerInputHandler playerInputHandler = instance.GetComponent<PlayerInputHandler>();
            _playerInputHandlerProvider.Set(playerInputHandler);
            
            return instance;
        }

        public GameObject CreateCubeSpawner()
        {
            GameObject prefab = _staticData.GetPrefab(PrefabId.CubeSpawner);
            GameObject instance = _instantiator.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, SceneRoot());

            CubeSpawner cubeSpawner = instance.GetComponent<CubeSpawner>();
            cubeSpawner.Initialize();

            _cubeSpawnerProvider.Instance = cubeSpawner;

            return instance;
        }

        public GameObject CreateCube(int cubeValue)
        {
            CubeSpawnPoint spawnPoint = _cubeSpawnPointProvider.Instance;
            
            GameObject instance = _cubePool.Get(spawnPoint.transform.position, Quaternion.identity);
            
            CubeMover cubeMover = instance.GetComponent<CubeMover>();
            cubeMover.Initialize();

            Cube cube = instance.GetComponent<Cube>();
            cube.Initialize(cubeValue);

            return instance;
        }
        
        public GameObject CreateCube(int cubeValue, Vector3 at)
        {
            GameObject instance = _cubePool.Get(at, Quaternion.identity);

            Cube cube = instance.GetComponent<Cube>();
            cube.Initialize(cubeValue);

            return instance;
        }

        private Transform SceneRoot()
        {
            Transform root = _sceneProvider.Container;

            if (root == null)
                throw new InvalidOperationException($"{nameof(SceneContainer)} is not initialized in active scene.");

            return root;
        }
    }
}
