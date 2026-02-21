using System;
using DG.Tweening;
using Services.StaticData;
using Services.StaticData.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Elements
{
    public class ScoreView : MonoBehaviour, IScoreHudView
    {
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private Button _restartButton;

        public event Action RestartRequested;
        public event Action Destroyed;

        private bool _isConfigured;
        private float _punchScale;
        private float _scaleDuration;
        private float _countDuration;

        private int _currentValue;

        private Tween _countTween;
        private Tween _scaleTween;

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            ScoreViewStaticData config = staticData.ScoreViewConfig;

            if (config == null)
                throw new InvalidOperationException($"{nameof(ScoreViewStaticData)} is not initialized.");

            _punchScale = config.PunchScale;
            _scaleDuration = config.ScaleDuration;
            _countDuration = config.CountDuration;
            _isConfigured = true;
        }

        public void SetScore(int newValue)
        {
            if (_isConfigured == false)
                throw new InvalidOperationException($"{nameof(ScoreViewStaticData)} is not configured.");

            _countTween?.Kill();
            _scaleTween?.Kill();

            _countTween = DOTween.To(
                () => _currentValue,
                x => {
                    _currentValue = x;
                    _counter.text = x.ToString();
                },
                newValue,
                _countDuration
            ).SetEase(Ease.OutQuad);

            _scaleTween = _counter.transform
                .DOScale(_punchScale, _scaleDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() => _counter.transform.DOScale(endValue: 1f, duration: _scaleDuration / 2)
                    .SetEase(Ease.InBack));
        }

        private void OnEnable()
        {
            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnDisable()
        {
            if (_restartButton != null)
                _restartButton.onClick.RemoveListener(OnRestartClicked);
        }

        private void OnDestroy()
        {
            _countTween?.Kill();
            _scaleTween?.Kill();
            Destroyed?.Invoke();
        }

        private void OnRestartClicked() =>
            RestartRequested?.Invoke();
    }
}
