using System;
using System.Collections.Generic;
using Gameplay.Cubes;
using Services.Scene;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Services.CubePools
{
    public class CubePool : ICubePool
    {
        public event System.Action<Cube> CubeAcquired;
        public event System.Action<Cube> CubeReleased;

        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        private readonly ISceneProvider _sceneProvider;
        private readonly Stack<GameObject> _pool = new();
        
        private GameObject _prefab;

        public CubePool(IInstantiator instantiator, IStaticDataService staticData, ISceneProvider sceneProvider)
        {
            _instantiator = instantiator;
            _staticData = staticData;
            _sceneProvider = sceneProvider;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            _prefab ??= _staticData.GetPrefab(PrefabId.Cube);

            GameObject cube = TryGetFromPool();

            if (cube == null)
                cube = _instantiator.InstantiatePrefab(_prefab, position, rotation, SceneRoot());

            cube.transform.SetPositionAndRotation(position, rotation);
            cube.SetActive(true);

            Cube cubeComponent = cube.GetComponent<Cube>();
            CubeAcquired?.Invoke(cubeComponent);
            
            return cube;
        }

        public void Release(GameObject cubeObject)
        {
            if (cubeObject == null)
                return;

            Cube cube = cubeObject.GetComponent<Cube>();
            CubeReleased?.Invoke(cube);

            cubeObject.SetActive(false);
            cube.Cleanup();

            _pool.Push(cubeObject);
        }

        public void Clear() =>
            _pool.Clear();

        private GameObject TryGetFromPool()
        {
            while (_pool.Count > 0)
            {
                GameObject cube = _pool.Pop();

                if (cube != null)
                    return cube;
            }

            return null;
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
