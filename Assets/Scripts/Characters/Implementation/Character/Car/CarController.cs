using Game.Data.DataFields.Implementation;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Characters.Character.Car
{
    public class CarController : CharacterController
    {
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] private WheelController[] _wheelControllers;
        [SerializeField] private float _speedMax = 2000f;
        [SerializeField] private float _speedAcceleration = 30f;
        [SerializeField] private float _angleMax = 40f;
        [SerializeField] private float _angleAcceleration = 20f;

        [SerializeField] protected bool _isDriving = true;
        
        protected VelocityData _velocityData;
        protected DirectionData _directionData;

        public WheelController[] WheelController => _wheelControllers;
        public Rigidbody Rigidbody => _rigidbody;
        
        public bool IsDriving => _isDriving;
        
        protected override void Awake()
        {
            base.Awake();

            _velocityData = GetDataField<VelocityData>();
            _directionData = GetDataField<DirectionData>();
        }

        private void Update()
        {
            if (_isDriving)
            {
                Rotate(_directionData.Value.x, Time.deltaTime);
                Move(_directionData.Value.y, Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if(!_rigidbody.isKinematic) _velocityData?.SetValue(_rigidbody.velocity.sqrMagnitude);
        }

        public void Rotate(int direction, float speedFactor)
        {
            _wheelControllers.ForEach(wheel => wheel.SetSteerAngle(_angleMax * direction, _angleAcceleration * speedFactor));
        }
        
        public void Move(int direction, float speedFactor)
        {
            float targetSpeed = _speedMax * direction;
            
            _wheelControllers.ForEach(wheel => wheel.SetMotorTorque(targetSpeed, _speedAcceleration * speedFactor));
            
            _rigidbody.drag = targetSpeed <= 0f ? 0.3f : 0f;
        }

        public virtual void SetDriving(bool isDriving)
        {
            _isDriving = isDriving;
        }
    }
}