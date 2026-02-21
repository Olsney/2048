using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.StaticData.Configs
{
    [Serializable]
    public class PrefabConfigEntry
    {
        [SerializeField] private PrefabId _id;
        [SerializeField] private GameObject _prefab;

        public PrefabId Id => _id;
        public GameObject Prefab => _prefab;
    }

    [CreateAssetMenu(fileName = "PrefabsStaticData", menuName = "Static Data/Prefabs")]
    public class PrefabStaticData : ScriptableObject
    {
        [SerializeField] private List<PrefabConfigEntry> _entries = new();

        public IReadOnlyList<PrefabConfigEntry> Entries => _entries;
    }
}
