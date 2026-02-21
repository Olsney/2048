using System;
using Cysharp.Threading.Tasks;
using Services.Scene;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Services.Merge
{
    public class MergeVfxService : IMergeVfxService
    {
        private const float VfxTimeoutSeconds = 5f;

        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        private readonly ISceneProvider _sceneProvider;

        public MergeVfxService(IInstantiator instantiator, IStaticDataService staticData, ISceneProvider sceneProvider)
        {
            _instantiator = instantiator;
            _staticData = staticData;
            _sceneProvider = sceneProvider;
        }

        public void PlayAt(Vector3 position)
        {
            GameObject prefab = _staticData.GetPrefab(PrefabId.MergeVfx);
            GameObject instance = _instantiator.InstantiatePrefab(prefab, position, Quaternion.identity, SceneRoot());

            DestroyAfterPlayback(instance).Forget();
        }

        private async UniTaskVoid DestroyAfterPlayback(GameObject instance)
        {
            if (instance == null)
                return;

            ParticleSystem[] particleSystems = instance.GetComponentsInChildren<ParticleSystem>(true);

            if (particleSystems.Length == 0)
            {
                UnityEngine.Object.Destroy(instance);

                return;
            }

            UniTask particlesTask = UniTask.WaitUntil(() => AreAllParticlesStopped(particleSystems));
            UniTask timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(VfxTimeoutSeconds), DelayType.Realtime);

            await UniTask.WhenAny(particlesTask, timeoutTask);

            if (instance != null)
                UnityEngine.Object.Destroy(instance);
        }

        private Transform SceneRoot()
        {
            Transform root = _sceneProvider.Container;

            if (root == null)
                throw new InvalidOperationException($"{nameof(SceneContainer)} is not initialized in active scene.");

            return root;
        }

        private static bool AreAllParticlesStopped(ParticleSystem[] particleSystems)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem != null && particleSystem.IsAlive(true))
                    return false;
            }

            return true;
        }
    }
}
