using System;
using System.Collections.Generic;
using UI.Services.Windows;
using UnityEngine;

namespace Services.StaticData.Configs
{
    [Serializable]
    public class WindowConfigEntry
    {
        [SerializeField] private WindowType _windowType;
        [SerializeField] private GameObject _prefab;

        public WindowType WindowType => _windowType;
        public GameObject Prefab => _prefab;
    }

    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "Static Data/Windows")]
    public class WindowStaticData : ScriptableObject
    {
        [SerializeField] private List<WindowConfigEntry> _entries = new();

        public IReadOnlyList<WindowConfigEntry> Entries => _entries;
    }
}
