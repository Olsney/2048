using Services.StaticData.Configs;
using UnityEngine;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        CubeGameplayStaticData CubeConfig { get; }
        ScoreViewStaticData ScoreViewConfig { get; }

        void LoadAll();
        GameObject GetPrefab(PrefabId id);
    }
}
