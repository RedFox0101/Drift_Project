using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Levels.Core;
using Game.UI;
using Game.UI.Base;
using UnityEngine;
using Zenject;

namespace Game.Levels.Handlers
{
    public class LevelTimerHandler : IHandlers<LevelController>
    {
        [SerializeField] private UIText _uiTextContract;
        
        private UIManager _uiManager;
        private LevelsManager _levelsManager;

        private UIText _uiText;

        private CancellationTokenSource _cancellationTokenTimer;
        
        [Inject]
        private void Install(UIManager uiManager, LevelsManager levelsManager)
        {
            _uiManager = uiManager;
            _levelsManager = levelsManager;
        }
        
        private void OnEnable()
        {
            _targetData.OnLevelActiveChanged += OnLevelActiveChanged;
        }

        private void OnDisable()
        {
            _targetData.OnLevelActiveChanged -= OnLevelActiveChanged;
            _cancellationTokenTimer?.Cancel();

            if (_uiText != null)
            {
                _uiManager.HideElement(_uiText);
                _uiText = null;
            }
        }

        private void OnLevelActiveChanged(bool isActive)
        {
            if (!isActive) return;

            var levelData = _levelsManager.GetData(_targetData.LevelDataID);

            if (levelData != null)
            {
                _uiText = _uiManager.ShowElement(_uiTextContract);

                Timer(levelData.DurationTimer);
            }
        }

        private async void Timer(float duration)
        {
            _cancellationTokenTimer?.Cancel();
            _cancellationTokenTimer = new();
            
            var targetTime = DateTime.Now + TimeSpan.FromMinutes(duration);

            try
            {
                while (DateTime.Now < targetTime)
                {
                    var current = DateTime.Now - targetTime;

                    string text = current.ToString(@"mm\:ss");

                    _uiText.SetText(text);

                    await UniTask.Yield(cancellationToken: _cancellationTokenTimer.Token);
                }
                
                _uiManager.HideElement(_uiText);

                _uiText = null;
            
                _targetData.LevelFinish();
            }
            catch (OperationCanceledException) { }
        }
    }
}