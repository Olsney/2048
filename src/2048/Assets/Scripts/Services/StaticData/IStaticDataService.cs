using Services.StaticData.Configs;
using UI.Services.Windows;
using UnityEngine;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        CubeGameplayStaticData CubeConfig { get; }
        ScoreViewStaticData ScoreViewConfig { get; }

        void LoadAll();
        GameObject GetPrefab(PrefabId id);
        GameObject GetWindowPrefab(WindowType windowType);
    }
}
