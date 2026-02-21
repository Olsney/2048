using System;
using System.Collections.Generic;
using Services.StaticData.Configs;
using UI.Services.Windows;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string PrefabsStaticDataPath = "StaticData/Common/PrefabsStaticData";
        private const string CubeGameplayStaticDataPath = "StaticData/Gameplay/CubeGameplayStaticData";
        private const string ScoreViewStaticDataPath = "StaticData/UI/ScoreViewStaticData";
        private const string WindowStaticDataPath = "StaticData/Window/WindowStaticData";

        private readonly Dictionary<PrefabId, GameObject> _prefabs = new();
        private readonly Dictionary<WindowType, GameObject> _windowPrefabs = new();
        private bool _isLoaded;

        public CubeGameplayStaticData CubeConfig { get; private set; }
        public ScoreViewStaticData ScoreViewConfig { get; private set; }

        public void LoadAll()
        {
            if (_isLoaded)
                return;

            LoadPrefabs();
            LoadWindows();
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

        public GameObject GetWindowPrefab(WindowType windowType)
        {
            EnsureLoaded();

            if (_windowPrefabs.TryGetValue(windowType, out GameObject prefab))
                return prefab;

            throw new KeyNotFoundException($"Window prefab with type {windowType} is not configured.");
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

        private void LoadWindows()
        {
            WindowStaticData config = LoadConfig<WindowStaticData>(WindowStaticDataPath);

            foreach (WindowConfigEntry entry in config.Entries)
            {
                if (entry.WindowType == WindowType.Unknown)
                    throw new InvalidOperationException("Window type cannot be Unknown.");

                if (entry.Prefab == null)
                    throw new InvalidOperationException($"Window prefab for type {entry.WindowType} is null.");

                if (_windowPrefabs.TryAdd(entry.WindowType, entry.Prefab) == false)
                    throw new InvalidOperationException($"Duplicate window type found: {entry.WindowType}.");
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
