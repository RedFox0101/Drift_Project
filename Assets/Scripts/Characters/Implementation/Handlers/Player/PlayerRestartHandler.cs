using Game.Characters.Character;
using UnityEngine;

namespace Game.Characters.Handlers.Player
{
    public class PlayerRestartHandler : IHandlers<PlayerController>
    {
        [SerializeField] private float _radius;
        [SerializeField] private Vector3 _offset;
        
        private RaycastHit[] _hit = new RaycastHit[1];
        
        private void Update()
        {
            if (UnityEngine.Input.GetKeyUp(KeyCode.R))
            {
                Vector3 position = Vector3.one;
                var i = Physics.SphereCastNonAlloc(_targetData.transform.position, _radius, Vector3.forward, _hit);
                if (i > 0)
                {
                    position = _hit[0].point + _offset;
                }

                _targetData.transform.position = position;
            }
        }
    }
}