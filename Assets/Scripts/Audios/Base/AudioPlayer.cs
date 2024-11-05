using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Assets;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Game.Audios
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IAudio
    {
        [SerializeField] private AudioType _audioType;
        [SerializeField] protected AudioSource _audioSource;
        
        private AudioManager _audioManager;
        private CancellationTokenSource _cancellationTokenWaitEnd;
        
        public GameObject Asset => gameObject;
        public event Action<IAsset> OnReleased;

        [Inject]
        private void Install(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void OnEnable()
        {
            _audioManager.OnAudioTypeEnabledChanged += AudioTypeEnabledChanged;
            
            AudioTypeEnabledChanged(_audioType, _audioManager.GetTypeStatus(_audioType));
        }

        protected virtual void OnDisable()
        {
            _audioManager.OnAudioTypeEnabledChanged -= AudioTypeEnabledChanged;
            OnReleased?.Invoke(this);
        }
        
        private void AudioTypeEnabledChanged(AudioType audioType, bool isEnabled)
        {
            if (!_audioType.Equals(audioType)) return;

            _audioSource.mute = !isEnabled;
        }

        public virtual void Play()
        {
            _audioSource.Play();
            WaitEndPlaying();
        }
        
        public virtual void Stop()
        {
            if(_audioSource.IsDestroyed() || _audioSource.IsUnityNull()) return; 
            
            _audioSource.Stop();
            
            gameObject.SetActive(false);
        }

        protected virtual async void WaitEndPlaying()
        {
            _cancellationTokenWaitEnd?.Cancel();
            _cancellationTokenWaitEnd = new CancellationTokenSource();
            try
            {
                await UniTask.WaitWhile(IsPlaying, cancellationToken: _cancellationTokenWaitEnd.Token);
                Stop();
            }
            catch(OperationCanceledException){}
        }

        public bool IsPlaying()
        {
            return !_audioSource.IsDestroyed() && _audioSource.isPlaying;
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public float GetVolume()
        {
            return _audioSource.volume;
        }
    }
}