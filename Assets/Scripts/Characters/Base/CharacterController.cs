using System;
using Game.Assets;
using Game.Data;
using UnityEngine;

namespace Game.Characters
{
    public class CharacterController : DataController, ICharacter
    { 
        public Transform Transform => transform;

        public GameObject Asset => gameObject;
        public event Action<IAsset> OnReleased;
        
        protected virtual void Awake()
        {
            
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            OnReleased?.Invoke(this);
        }

        public virtual void Move(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            transform.position += direction;
        }

        public virtual void SetRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
        }
    }
}