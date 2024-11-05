using Cysharp.Threading.Tasks;
using Game.Assets;
using Game.Characters;
using Game.Levels.Core;
using Game.Loadings;
using Game.Server;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Levels.Handlers
{
    public class LevelStartHandler : IHandlers<LevelController>
    {
        [SerializeField] private UIButton _buttonStartContract;

        private UIManager _uiManager;
        private ServerManager _serverManager;
        private LoadingManager _loadingManager;
        
        private UIButton _buttonStart;
        
        [Inject]
        private void Install(ServerManager serverManager, LoadingManager loadingManager, UIManager uiManager)
        {
            _uiManager = uiManager;
            _serverManager = serverManager;
            _loadingManager = loadingManager;
        }

        private void Start()
        {
            if (_serverManager.IsMaster())
            {
                _targetData.SetActiveLevel(false);
                WaitLoadings();
            }
        }

        private void OnDisable()
        {
            if(_buttonStart != null) _uiManager.HideElement(_buttonStart);
        }

        private async void WaitLoadings()
        {
            await UniTask.WaitWhile(() => _loadingManager.IsLoading);
            _buttonStart = _uiManager.ShowElement(_buttonStartContract);

            _buttonStart.OnTap += OnStartClick;
            _buttonStart.OnReleased += OnButtonRelease;
        }
        
        private void OnStartClick()
        {
            _targetData.SetActiveLevel(true);
            _uiManager.HideElement(_buttonStart);
        }
        
        private void OnButtonRelease(IAsset asset)
        {
            asset.OnReleased -= OnButtonRelease;
            
            if (_buttonStart.Equals(asset))
            {
                _buttonStart = null;
            }
        }
    }
}