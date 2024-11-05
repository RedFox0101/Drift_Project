using Game.Characters.Character;
using Game.Data.DataFields.Implementation;
using UnityEngine;

namespace Game.Characters.Handlers.Player
{
    public class InputHandler : IHandlers<PlayerController>
    {
        [SerializeField] private float _threshold = 0.1f;
        
        private DirectionData _directionData;

        private void Awake()
        {
            _directionData = _targetData.GetDataField<DirectionData>();
        }

        private void Update()
        {
            var direction = Vector2Int.zero;
            
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (Mathf.Abs(x) > _threshold) direction.x = x > 0f ? 1 : -1;
            if (Mathf.Abs(y) > _threshold) direction.y = y > 0f ? 1 : -1;
            
            _directionData.SetValue(direction);
        }
    }
}