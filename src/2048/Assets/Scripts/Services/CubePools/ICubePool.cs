using System;
using Gameplay.Cubes;
using UnityEngine;

namespace Services.CubePools
{
    public interface ICubePool
    {
        event Action<Cube> CubeAcquired;
        event Action<Cube> CubeReleased;

        GameObject Get(Vector3 position, Quaternion rotation);
        void Release(GameObject cubeObject);
        void Clear();
    }
}
