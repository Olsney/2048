using Services.StaticData;
using UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        
        private GameObject _uiRoot;

        public UIFactory(IInstantiator instantiator, IStaticDataService staticData)
        {
            _instantiator = instantiator;
            _staticData = staticData;
        }
        
        public GameObject CreateUIRoot()
        {
            GameObject prefab = _staticData.GetPrefab(PrefabId.UIRoot);
            _uiRoot = _instantiator.InstantiatePrefab(prefab);
            
            return _uiRoot;
        }

        public GameObject CreateHud()
        {
            GameObject prefab = _staticData.GetPrefab(PrefabId.Hud);
            
            return _instantiator.InstantiatePrefab(prefab);
        }

        public GameObject CreateVictoryWindow()
        {
            GameObject prefab = _staticData.GetWindowPrefab(WindowType.VictoryWindow);
            
            return _instantiator.InstantiatePrefab(prefab,_uiRoot.transform);
            
        }

        public GameObject CreateLoseWindow()
        {
            GameObject prefab = _staticData.GetWindowPrefab(WindowType.LoseWindow);
            
            return _instantiator.InstantiatePrefab(prefab, _uiRoot.transform);        
        }
    }
}
