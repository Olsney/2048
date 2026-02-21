using UnityEngine;

namespace Services.StaticData.Configs
{
    [CreateAssetMenu(fileName = "ScoreViewStaticData", menuName = "Static Data/Score View")]
    public class ScoreViewStaticData : ScriptableObject
    {
        [SerializeField] private float _punchScale = 1.2f;
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _countDuration = 0.3f;

        public float PunchScale => _punchScale;
        public float ScaleDuration => _scaleDuration;
        public float CountDuration => _countDuration;
    }
}
