using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Assets;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Audios
{
    public class AudioPlaylist : MonoBehaviour, IAudio
    {
        [SerializeField] private List<AudioPlayer> _queueAudioPlayer;
        [SerializeField] private bool _loop;
        
        private Queue<AudioPlayer> _queueAudio = new Queue<AudioPlayer>();
        private AudioPlayer _currentAudioPlayer;
        private CancellationTokenSource _cancellationTokenPlaylist;
        
        public GameObject Asset => gameObject;
        public event Action<IAsset> OnReleased;

        private void Awake()
        {
            if (!_queueAudioPlayer.IsNullOrEmpty())
            {
                _queueAudio = new Queue<AudioPlayer>(_queueAudioPlayer);
            }
        }

        protected virtual void Start()
        {
            
        }
        
        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            _cancellationTokenPlaylist?.Cancel();
            OnReleased?.Invoke(this);
        }

        public void Play()
        {
            Playlist();
        }

        public void Stop()
        {
            _cancellationTokenPlaylist?.Cancel();
            if (!_currentAudioPlayer.IsDestroyed())
            {
                _currentAudioPlayer.Stop();
            }
        }

        private async void Playlist()
        {
            _cancellationTokenPlaylist?.Cancel();
            _cancellationTokenPlaylist = new();
            try
            {
                while (_queueAudio.Count > 0 && !_cancellationTokenPlaylist.IsCancellationRequested)
                {
                    _currentAudioPlayer = _queueAudio.Dequeue();
                    
                    _currentAudioPlayer.gameObject.SetActive(true);
                    
                    _currentAudioPlayer.Play();

                    await UniTask.WaitWhile(() => _currentAudioPlayer.IsPlaying(),
                        cancellationToken: _cancellationTokenPlaylist.Token);
                }

                if (_loop)
                {
                    _queueAudio = new Queue<AudioPlayer>(_queueAudioPlayer);
                    Playlist();
                }
            }
            catch(OperationCanceledException) {}
        }
    }
}