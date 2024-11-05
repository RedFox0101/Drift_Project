using System;
using Game.Audios;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.UIElements
{
    public class UIToggle : UIElement
    {
        [SerializeField] protected Toggle _toggle;
        [SerializeField] private AudioPlayer _audioPlayerContract;
        
        private AudioManager _audioManager;
        
        public event Action<bool> OnTap;
        
        [Inject]
        private void Install(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        protected virtual void OnEnable()
        {
            _toggle.onValueChanged.AddListener(ToggleChanged);
        }

        protected override void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(ToggleChanged);
            base.OnDisable();
        }

        private void ToggleChanged(bool value)
        {
            if (_audioPlayerContract != null) _audioManager.Play(_audioPlayerContract);

            OnToggleChanged(value);
        }

        protected virtual void OnToggleChanged(bool value)
        {
            OnTap?.Invoke(value);
        }
    }
}