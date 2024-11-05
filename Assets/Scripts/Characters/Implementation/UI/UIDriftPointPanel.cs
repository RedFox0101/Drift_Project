using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Characters.UI
{
    public class UIDriftPointPanel : UIElement
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private string _defaultText = "Drift Point:";
        [SerializeField] private float _timerDuration;

        private UIManager _uiManager;

        private CancellationTokenSource _cancellationTokenTimer;

        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        private void OnEnable()
        {
            Timer();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _cancellationTokenTimer?.Cancel();
        }

        public void SetPoints(int count)
        {
            _textField.text = $"{_defaultText} {count}";
            Timer();
        }

        private async void Timer()
        {
            _cancellationTokenTimer?.Cancel();
            _cancellationTokenTimer = new();
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_timerDuration),
                    cancellationToken: _cancellationTokenTimer.Token);
                
                _uiManager.HideElement(this);
            }
            catch(OperationCanceledException){}
        }
    }
}