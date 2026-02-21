using UnityEngine;

namespace Services.Scene
{
    public interface ISceneProvider
    {
        Transform Container { get; }
        void SetTransform(Transform container);
    }
}
