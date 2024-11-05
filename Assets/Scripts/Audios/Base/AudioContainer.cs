using UnityEngine;
using Zenject;

namespace Game.Audios
{
    public class AudioContainer : MonoBehaviour
    {
        private AudioManager _audioManager;

        [Inject]
        private void Install(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        private void OnEnable()
        {
            _audioManager.RegisterContainer(this);
        }

        private void OnDisable()
        {
            _audioManager.UnregisterContainer(this);
        }
    }
}