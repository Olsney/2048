using Data;
using Gameplay.Cubes;
using Services.CubePools;
using Services.CubeSpawnerProviders;
using UnityEngine;

namespace Services.Merge
{
    public class MergeService : IMergeService
    {
        private readonly ICubeSpawnerProvider _spawnerProvider;
        private readonly IWorldData _worldData;
        private readonly ICubePool _cubePool;
        private readonly IMergeVfxService _mergeVfxService;


        public MergeService(ICubeSpawnerProvider spawnerProvider,
            IWorldData worldData,
            ICubePool cubePool,
            IMergeVfxService mergeVfxService)
        {
            _spawnerProvider = spawnerProvider;
            _worldData = worldData;
            _cubePool = cubePool;
            _mergeVfxService = mergeVfxService;
        }

        public void Merge(Cube first, Cube second, Vector3 mergeEffectPosition)
        {
            if (first == null || second == null)
                return;

            first.MarkAsMerging();
            second.MarkAsMerging();

            int newCubeValue = first.Value + second.Value;
            int scoreReward = CalculateScoreReward(newCubeValue);

            _worldData.AddScore(scoreReward);

            Vector3 spawnPosition = GetSpawnPosition(first, second);
            _mergeVfxService.PlayAt(mergeEffectPosition);
            _spawnerProvider.Instance.SpawnMerge(newCubeValue, spawnPosition);

            ReleaseCube(first);
            ReleaseCube(second);
        }
        
        private void ReleaseCube(Cube cube)
        {
            if (cube.TryGetComponent(out CubeMover mover))
                mover.Cleanup();

            _cubePool.Release(cube.gameObject);
        }

        private static int CalculateScoreReward(int mergedValue) => 
            mergedValue / 2;

        private static Vector3 GetSpawnPosition(Cube first, Cube second) =>
            (first.transform.position + second.transform.position) / 2f;
    }
}
