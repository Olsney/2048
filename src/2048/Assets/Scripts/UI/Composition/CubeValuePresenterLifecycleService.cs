using System;
using System.Collections.Generic;
using Gameplay.Cubes;
using Services.CubePools;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace UI.Composition
{
    public sealed class CubeValuePresenterLifecycleService : IInitializable, IDisposable
    {
        private readonly ICubePool _cubePool;
        private readonly IUIPresenterFactory _presenterFactory;
        private readonly Dictionary<Cube, CubeValuePresenter> _presenters = new();
        private readonly Dictionary<Cube, Action> _destroyHandlers = new();

        public CubeValuePresenterLifecycleService(ICubePool cubePool, IUIPresenterFactory presenterFactory)
        {
            _cubePool = cubePool;
            _presenterFactory = presenterFactory;
        }

        public void Initialize()
        {
            _cubePool.CubeAcquired += OnCubeAcquired;
            _cubePool.CubeReleased += OnCubeReleased;
        }

        public void Dispose()
        {
            _cubePool.CubeAcquired -= OnCubeAcquired;
            _cubePool.CubeReleased -= OnCubeReleased;

            foreach (KeyValuePair<Cube, Action> pair in _destroyHandlers)
            {
                if (pair.Key != null)
                    pair.Key.Destroyed -= pair.Value;
            }

            _destroyHandlers.Clear();

            foreach (CubeValuePresenter presenter in _presenters.Values)
                presenter.Dispose();

            _presenters.Clear();
        }

        private void OnCubeAcquired(Cube cube)
        {
            if (cube == null)
                return;

            CubeValuePresenter presenter = EnsurePresenter(cube);
            presenter.Initialize();
        }

        private void OnCubeReleased(Cube cube)
        {
            if (cube == null)
                return;

            if (_presenters.TryGetValue(cube, out CubeValuePresenter presenter))
                presenter.Dispose();
        }

        private CubeValuePresenter EnsurePresenter(Cube cube)
        {
            if (_presenters.TryGetValue(cube, out CubeValuePresenter presenter))
                return presenter;

            ICubeValueView view = ResolveView<ICubeValueView>(cube.gameObject);

            if (view == null)
                throw new InvalidOperationException($"{nameof(ICubeValueView)} is missing on cube prefab.");

            presenter = _presenterFactory.CreateCubeValuePresenter(cube, view);
            _presenters.Add(cube, presenter);

            Action destroyHandler = null;
            destroyHandler = () =>
            {
                cube.Destroyed -= destroyHandler;
                _destroyHandlers.Remove(cube);

                if (_presenters.Remove(cube, out CubeValuePresenter removedPresenter))
                    removedPresenter.Dispose();
            };

            _destroyHandlers.Add(cube, destroyHandler);
            cube.Destroyed += destroyHandler;

            return presenter;
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
    }
}
