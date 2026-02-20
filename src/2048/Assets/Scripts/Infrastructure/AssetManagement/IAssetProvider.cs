using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Load(string path);
    }
}