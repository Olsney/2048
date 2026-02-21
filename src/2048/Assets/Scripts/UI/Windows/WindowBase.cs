using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public event Action CloseRequested;
        public event Action Destroyed;
        
        [SerializeField] private Button _closeButton;

        protected Button CloseButton => _closeButton;
       
        protected virtual void Awake()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(HandleCloseClicked);
        }
        
        protected virtual void OnDestroy()
        {
            if (_closeButton != null)
                _closeButton.onClick.RemoveListener(HandleCloseClicked);

            Destroyed?.Invoke();
        }

        private void HandleCloseClicked() => 
            CloseRequested?.Invoke();

        public void Close() =>
            Destroy(gameObject);
    }
}
