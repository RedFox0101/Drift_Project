using Game.Characters.Character;
using UnityEngine;

namespace Game.Characters.Handlers.Player
{
    public class PlayerCameraHandler : IHandlers<PlayerController>
    {
        [SerializeField] private Transform _cameraTargetPoint;
        [SerializeField] private float _thresholdVelocity = 1f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _speed = 500f;
        
        private void LateUpdate()
        {
            _cameraTargetPoint.position = Vector3.Lerp(_cameraTargetPoint.position, _targetData.transform.position, _speed * 1000 * Time.deltaTime);

            Quaternion targetRotation;
            
            if (_targetData.Rigidbody.velocity.magnitude < _thresholdVelocity)
                targetRotation = Quaternion.LookRotation(_targetData.transform.forward);
            else
                targetRotation = Quaternion.LookRotation(_targetData.Rigidbody.velocity.normalized);
            
            targetRotation = Quaternion.Slerp(_cameraTargetPoint.rotation, targetRotation, _rotationSpeed * Time.deltaTime);                
            
            _cameraTargetPoint.rotation = targetRotation;
        }
    }
}