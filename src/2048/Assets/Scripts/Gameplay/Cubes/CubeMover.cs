using System;
using Gameplay.Input;
using Services.InputHandlerProviders;
using UnityEngine;
using Zenject;

namespace Gameplay.Cubes
{
    public class CubeMover : MonoBehaviour
    {
        [SerializeField] private float _leftLimitZ = -0.6f;
        [SerializeField] private float _rightLimitZ = 2.7f;
        [SerializeField] private float _distanceFromCamera = 10f;
        [SerializeField] private float _launchForce = 500f;

        private Rigidbody _rigidbody;
        private Camera _mainCamera;
        private bool _isDragging;
        private bool _isLaunched;

        private IPlayerInputHandlerProvider _inputProvider;
        private IPlayerInputEvents _input;

        [Inject]
        public void Construct(IPlayerInputHandlerProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        public void Initialize()
        {
            _input = _inputProvider.Get();
            
            if (_input == null)
                throw new InvalidOperationException($"{nameof(IPlayerInputEvents)} is not initialized.");

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _isDragging = false;
            _isLaunched = false;
            
            _mainCamera = Camera.main;

            _input.TapStarted += OnTapStarted;
            _input.TapEnded += OnTapEnded;
        }
        
        public void Cleanup()
        {
            if (_input == null)
                return;

            _input.TapStarted -= OnTapStarted;
            _input.TapEnded -= OnTapEnded;
        }

        private void OnDestroy() => 
            Cleanup();

        private void Update()
        {
            if (_isDragging && !_isLaunched)
                DragWithPointer(_input.PointerPosition());
        }

        private void OnTapStarted(Vector2 pos)
        {
            if (_isLaunched) 
                return;
            
            _isDragging = true;
        }

        private void OnTapEnded(Vector2 pos)
        {
            if (_isLaunched) 
                return;

            _isDragging = false;
            _isLaunched = true;

            _rigidbody.isKinematic = false;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _rigidbody.AddForce(Vector3.left * _launchForce, ForceMode.Impulse);
        }

        private void DragWithPointer(Vector2 screenPosition)
        {
            Vector3 cameraPosition = _mainCamera.ScreenToWorldPoint(new Vector3(
                screenPosition.x, screenPosition.y, _distanceFromCamera));
            Vector3 cubePermissiblePosition = transform.position;
            cubePermissiblePosition.z = Mathf.Clamp(
                cameraPosition.z, 
                _leftLimitZ, 
                _rightLimitZ);
            
            transform.position = cubePermissiblePosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_rigidbody == null)
                return;
            
            if (other.TryGetComponent(out CubeRotationActivator _)) 
                _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
