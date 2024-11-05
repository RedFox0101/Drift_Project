using System;
using Game.Audios;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class UIButton : UIElement
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioPlayer _audioPlayerContract;

        private AudioManager _audioManager;
        
        public event Action OnTap;

        [Inject]
        private void Install(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(Click);
        }

        protected override void OnDisable()
        {
            _button.onClick.RemoveListener(Click);
            base.OnDisable();
        }

        private void Click()
        {
            if (_audioPlayerContract != null) _audioManager.Play(_audioPlayerContract);
            
            OnClick();
        }
        
        protected virtual void OnClick()
        {
            OnTap?.Invoke();
        }
    }
}