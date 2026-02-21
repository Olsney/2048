using UnityEngine;

namespace Services.Scene
{
    public class SceneProvider : ISceneProvider
    {
        public Transform Container { get; private set; }

        public void SetTransform(Transform container) =>
            Container = container;
    }
}
