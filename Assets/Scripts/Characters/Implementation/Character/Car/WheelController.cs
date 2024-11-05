using UnityEngine;

namespace Game.Characters.Character.Car
{
    [RequireComponent(typeof(WheelSkid))]
    [RequireComponent(typeof(WheelCollider))]
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private WheelSkid _wheelSkid;
        [SerializeField] private Transform _visual;
        [SerializeField] private WheelCollider _wheelCollider;
        [SerializeField] private bool _isSteerable;
        [SerializeField] private bool _isMotor;
        [SerializeField] private float _maxSpeed = 200f;
        [SerializeField] private float _speedFactor = 0.5f;

        public WheelSkid WheelSkid => _wheelSkid;
        
        private void Awake()
        {
            _wheelSkid ??= GetComponent<WheelSkid>();
            _wheelCollider ??= GetComponent<WheelCollider>();
        }
        
        private void LateUpdate()
        {
            _wheelCollider.GetWorldPose(out var position, out var rotation);
            
            _visual.position = position;
            
            _visual.localEulerAngles = new Vector3(_visual.localEulerAngles.x, _wheelCollider.steerAngle - _visual.localEulerAngles.z, _visual.localEulerAngles.z);

            float rpm = _wheelCollider.rpm;
            rpm = Mathf.Clamp(rpm, -_maxSpeed, _maxSpeed);
            
            _visual.Rotate(rpm * _speedFactor / 60 * 360 * Time.deltaTime, 0, 0);
        }

        public void SetSteerAngle(float targetSteerAngle, float speed)
        {
            if (_isSteerable && _wheelCollider.steerAngle != targetSteerAngle) 
            {
                _wheelCollider.steerAngle = Mathf.LerpAngle(_wheelCollider.steerAngle, targetSteerAngle, speed);
            }
        }
        
        public void SetMotorTorque(float targetSpeed, float speed)
        {
            if (_isMotor && _wheelCollider.motorTorque != targetSpeed)
            {
                _wheelCollider.motorTorque = Mathf.Lerp(_wheelCollider.motorTorque, targetSpeed, speed);
            }
        }
    }
}