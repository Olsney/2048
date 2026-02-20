using UnityEngine;

namespace Services.StaticData.Configs
{
    [CreateAssetMenu(fileName = "CubeGameplayStaticData", menuName = "Static Data/Cube Gameplay")]
    public class CubeGameplayStaticData : ScriptableObject
    {
        [SerializeField] private float _leftLimitZ = -0.6f;
        [SerializeField] private float _rightLimitZ = 2.7f;
        [SerializeField] private float _distanceFromCamera = 10f;
        [SerializeField] private float _launchForce = 15f;
        [SerializeField] private float _minMergeImpulse = 0.1f;

        public float LeftLimitZ => _leftLimitZ;
        public float RightLimitZ => _rightLimitZ;
        public float DistanceFromCamera => _distanceFromCamera;
        public float LaunchForce => _launchForce;
        public float MinMergeImpulse => _minMergeImpulse;
    }
}
