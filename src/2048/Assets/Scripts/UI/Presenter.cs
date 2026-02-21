using System;
using Zenject;

namespace UI
{
    public abstract class Presenter<TView> : IInitializable, IDisposable
    {
        private bool _isInitialized;

        protected Presenter(TView view)
        {
            View = view;
        }

        protected TView View { get; }

        public void Initialize()
        {
            if (_isInitialized)
                return;

            OnInitialize();
            _isInitialized = true;
        }

        public void Dispose()
        {
            if (_isInitialized == false)
                return;

            OnDispose();
            _isInitialized = false;
        }

        protected abstract void OnInitialize();
        protected abstract void OnDispose();
    }
}
