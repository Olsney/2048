using UnityEngine;

namespace Services.Scene
{
  public interface ISceneProvider
  {
    void SetTransform(Transform container);
    Transform Container { get; }
  }
}