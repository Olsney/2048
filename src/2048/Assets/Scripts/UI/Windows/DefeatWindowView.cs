using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class DefeatWindowView : WindowBase, IDefeatWindowView
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _restartButton;

        public event Action RestartRequested;

        protected override void Awake()
        {
            base.Awake();

            if (_restartButton == null)
                return;

            _restartButton.onClick.AddListener(OnRestartClicked);
        }

        protected override void OnDestroy()
        {
            if (_restartButton != null)
                _restartButton.onClick.RemoveListener(OnRestartClicked);

            base.OnDestroy();
        }

        public void SetMessage(string message) =>
            _text.text = message;

        private void OnRestartClicked() =>
            RestartRequested?.Invoke();
    }
}
