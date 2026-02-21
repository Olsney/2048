using System;
using Services.StaticData;
using UI.Elements;
using UI.Services.Windows;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        private readonly IUIPresenterFactory _presenterFactory;
        
        private GameObject _uiRoot;
        private ScoreHudPresenter _scoreHudPresenter;
        private IScoreHudView _scoreHudView;

        public UIFactory(IInstantiator instantiator, IStaticDataService staticData, IUIPresenterFactory presenterFactory)
        {
            _instantiator = instantiator;
            _staticData = staticData;
            _presenterFactory = presenterFactory;
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
            GameObject instance = _instantiator.InstantiatePrefab(prefab);

            IScoreHudView view = ResolveView<IScoreHudView>(instance);

            if (view == null)
                throw new InvalidOperationException($"{nameof(IScoreHudView)} is missing on HUD prefab.");

            if (_scoreHudView != null)
                _scoreHudView.Destroyed -= OnScoreHudDestroyed;
            
            _scoreHudPresenter?.Dispose();

            _scoreHudView = view;
            _scoreHudPresenter = _presenterFactory.CreateScoreHudPresenter(view);
            _scoreHudPresenter.Initialize();
            _scoreHudView.Destroyed += OnScoreHudDestroyed;
            
            return instance;
        }

        public GameObject CreateVictoryWindow()
        {
            GameObject prefab = _staticData.GetWindowPrefab(WindowType.VictoryWindow);
            GameObject instance = _instantiator.InstantiatePrefab(prefab, _uiRoot.transform);
            IVictoryWindowView view = ResolveView<IVictoryWindowView>(instance);

            if (view == null)
                throw new InvalidOperationException($"{nameof(IVictoryWindowView)} is missing on victory window prefab.");

            VictoryWindowPresenter presenter = _presenterFactory.CreateVictoryWindowPresenter(view);
            presenter.Initialize();

            view.Destroyed += OnDestroyed;
            
            return instance;

            void OnDestroyed()
            {
                view.Destroyed -= OnDestroyed;
                presenter.Dispose();
            }
            
        }

        public GameObject CreateDefeatWindow()
        {
            GameObject prefab = _staticData.GetWindowPrefab(WindowType.DefeatWindow);
            GameObject instance = _instantiator.InstantiatePrefab(prefab, _uiRoot.transform);
            IDefeatWindowView view = ResolveView<IDefeatWindowView>(instance);

            if (view == null)
                throw new InvalidOperationException($"{nameof(IDefeatWindowView)} is missing on defeat window prefab.");

            DefeatWindowPresenter presenter = _presenterFactory.CreateDefeatWindowPresenter(view);
            presenter.Initialize();

            view.Destroyed += OnDestroyed;
            
            return instance;

            void OnDestroyed()
            {
                view.Destroyed -= OnDestroyed;
                presenter.Dispose();
            }        
        }

        private static TView ResolveView<TView>(GameObject root) where TView : class
        {
            foreach (MonoBehaviour behaviour in root.GetComponentsInChildren<MonoBehaviour>(includeInactive: true))
            {
                if (behaviour is TView view)
                    return view;
            }

            return null;
        }

        private void OnScoreHudDestroyed()
        {
            if (_scoreHudView != null)
                _scoreHudView.Destroyed -= OnScoreHudDestroyed;

            _scoreHudPresenter?.Dispose();
            _scoreHudPresenter = null;
            _scoreHudView = null;
        }
    }
}
