using System;
using DG.Tweening;
using Services.StaticData;
using Services.StaticData.Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Elements
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

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

        public void SetValue(int newValue)
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
    }
}
