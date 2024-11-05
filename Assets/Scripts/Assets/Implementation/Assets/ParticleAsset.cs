using System;
using UnityEngine;

namespace Game.Assets.Assets
{
    public class ParticleAsset : MonoBehaviour, IAsset
    {
        [SerializeField] private ParticleSystem _particle;
        public GameObject Asset => gameObject;
        public event Action<IAsset> OnReleased;

        private void OnDisable()
        {
            OnReleased?.Invoke(this);
        }

        public void Play()
        {
            _particle.Play();
        }

        public void Stop()
        {
            _particle.Stop();
        }
    }
}