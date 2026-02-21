using Services.Scene;
using UnityEngine;
using Zenject;

public class SceneContainer : MonoBehaviour
{
    private ISceneProvider _sceneProvider;

    [Inject]
    public void Construct(ISceneProvider sceneProvider)
    {
        _sceneProvider = sceneProvider;
    }

    private void Awake()
    {
        _sceneProvider.SetTransform(transform);
    }

    private void OnDestroy()
    {
        if (_sceneProvider.Container == transform)
            _sceneProvider.SetTransform(null);
    }
}
