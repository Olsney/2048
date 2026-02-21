using System.Collections.Generic;
using Gameplay.Cubes;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Services.CubePools
{
    public class CubePool : ICubePool
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        private readonly Stack<GameObject> _pool = new();
        
        private GameObject _prefab;

        public CubePool(IInstantiator instantiator, IStaticDataService staticData)
        {
            _instantiator = instantiator;
            _staticData = staticData;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            _prefab ??= _staticData.GetPrefab(PrefabId.Cube);

            GameObject cube = _pool.Count > 0
                ? _pool.Pop()
                : _instantiator.InstantiatePrefab(_prefab, position, rotation, null);

            cube.transform.SetPositionAndRotation(position, rotation);
            cube.SetActive(true);
            
            return cube;
        }

        public void Release(GameObject cubeObject)
        {
            cubeObject.SetActive(false);

            Cube cube = cubeObject.GetComponent<Cube>();
            cube.Cleanup();

            _pool.Push(cubeObject);
        }
    }
}
