using System;
using Services.Merge;
using Services.StaticData;
using Services.StaticData.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Cubes
{
    public class Cube : MonoBehaviour
    {
        public event Action ValueUpdated;

        private bool _isConfigured;
        private float _minMergeImpulse;

        private IMergeService _mergeService;
        private Rigidbody _rigidbody;
        private Renderer _renderer;


        public bool IsMerging { get; private set; }
        public bool HasEnteredPlayArea { get; private set; }
        public bool HasTriggeredGameOver { get; private set; }
        
        public int Value { get; private set; }

        [Inject]
        public void Construct(IMergeService mergeService, IStaticDataService staticData)
        {
            _mergeService = mergeService;

            CubeGameplayStaticData config = staticData.CubeConfig;

            if (config == null)
                throw new InvalidOperationException($"{nameof(CubeGameplayStaticData)} is not initialized.");

            _minMergeImpulse = config.MinMergeImpulse;
            _isConfigured = true;
        }

        public void Initialize(int value)
        {
            if (_isConfigured == false)
                throw new InvalidOperationException($"{nameof(CubeGameplayStaticData)} is not configured.");

            Value = value;
            
            IsMerging = false;
            HasEnteredPlayArea = false;
            HasTriggeredGameOver = false;

            _rigidbody = GetComponent<Rigidbody>();
            
            _renderer = GetComponent<Renderer>();

            if (CubeColors.TryGet(Value, out Color color) && _renderer != null)
                _renderer.material.color = color;
            
            ValueUpdated?.Invoke();
        }

        public void Cleanup()
        {
            if (IsMerging)
                IsMerging = false;
            
            if (HasEnteredPlayArea)
                HasEnteredPlayArea = false;

            if (HasTriggeredGameOver)
                HasTriggeredGameOver = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsMerging)
                return;

            if (!collision.transform.TryGetComponent(out Cube cube))
                return;

            if (cube == this || cube.IsMerging)
                return;

            if (Value != cube.Value)
                return;

            Vector3 directionToOther = (cube.transform.position - transform.position).normalized;
            float velocityTowards = Vector3.Dot(_rigidbody.linearVelocity, directionToOther);

            if (velocityTowards < _minMergeImpulse)
                return;
            
            IsMerging = true;
            
            _mergeService.Merge(this, cube);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GameOverPoint losePoint) == false)
                return;

            if (HasEnteredPlayArea == false)
            {
                HasEnteredPlayArea = true;
                
                return;
            }

            if (HasEnteredPlayArea)
            {
                HasTriggeredGameOver = true;
                
                losePoint.Finish(cube: this);
            }
        }

        public void MarkAsMerging() => 
            IsMerging = true;

        public void MarkAsEnteredPlayArea() =>
            HasEnteredPlayArea = true;
    }
}
