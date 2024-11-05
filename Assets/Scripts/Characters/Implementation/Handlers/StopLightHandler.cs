using Game.Characters.Character.Car;
using Game.Data.DataFields.Implementation;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Characters.Handlers
{
    public class StopLightHandler : IHandlers<CarController>
    {
        [SerializeField] private Light[] _lights;

        private DirectionData _directionData;
        
        private void Awake()
        {
            _directionData = _targetData.GetDataField<DirectionData>();
        }

        private void OnEnable()
        {
            _directionData.OnChanged += OnDirectionChanged;
            OnDirectionChanged(_directionData.Value);
        }

        private void OnDisable()
        {
            _directionData.OnChanged -= OnDirectionChanged;
        }

        private void OnDirectionChanged(Vector2Int direction)
        {
            bool isActive = direction.y < 0f;

            _lights.ForEach(light => light.gameObject.SetActive(isActive));   
        }
    }
}