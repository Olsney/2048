using System;
using System.Collections.Generic;
using Services.StaticData.Configs;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string PrefabsStaticDataPath = "StaticData/PrefabsStaticData";
        private const string CubeGameplayStaticDataPath = "StaticData/CubeGameplayStaticData";
        private const string ScoreViewStaticDataPath = "StaticData/ScoreViewStaticData";

        private readonly Dictionary<PrefabId, GameObject> _prefabs = new();
        private bool _isLoaded;

        public CubeGameplayStaticData CubeConfig { get; private set; }
        public ScoreViewStaticData ScoreViewConfig { get; private set; }

        public void LoadAll()
        {
            if (_isLoaded)
                return;

            LoadPrefabs();
            CubeConfig = LoadConfig<CubeGameplayStaticData>(CubeGameplayStaticDataPath);
            ScoreViewConfig = LoadConfig<ScoreViewStaticData>(ScoreViewStaticDataPath);
            _isLoaded = true;
        }

        public GameObject GetPrefab(PrefabId id)
        {
            EnsureLoaded();

            if (_prefabs.TryGetValue(id, out GameObject prefab))
                return prefab;

            throw new KeyNotFoundException($"Prefab with id {id} is not configured.");
        }

        private void LoadPrefabs()
        {
            PrefabStaticData config = LoadConfig<PrefabStaticData>(PrefabsStaticDataPath);

            foreach (PrefabConfigEntry entry in config.Entries)
            {
                if (entry.Prefab == null)
                    throw new InvalidOperationException($"Prefab for id {entry.Id} is null.");

                if (_prefabs.TryAdd(entry.Id, entry.Prefab) == false)
                    throw new InvalidOperationException($"Duplicate prefab id found: {entry.Id}.");
            }
        }

        private static TConfig LoadConfig<TConfig>(string path) where TConfig : ScriptableObject
        {
            TConfig config = Resources.Load<TConfig>(path);

            if (config == null)
                throw new InvalidOperationException($"{typeof(TConfig).Name} is missing at Resources/{path}.");

            return config;
        }

        private void EnsureLoaded()
        {
            if (_isLoaded == false)
                throw new InvalidOperationException("Static data is not loaded. Call LoadAll() before usage.");
        }
    }
}
