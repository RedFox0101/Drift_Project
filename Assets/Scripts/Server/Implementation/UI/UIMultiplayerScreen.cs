using System;
using System.Threading;
using Game.UI;
using Game.UI.UIElements;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Game.Server.UI
{
    public class UIMultiplayerScreen : UIScreen
    {
        [SerializeField] private UICircle _loadingCircleContract;
        [SerializeField] private GameObject _panel;
        [SerializeField] private UIButton _createRoomButton;
        [SerializeField] private GameObject _createRoomPanel;
        [SerializeField] private UIButton _joinRoomButton;
        [SerializeField] private GameObject _joinRoomPanel;
        
        private UIManager _uiManager;
        private ServerManager _serverManager;

        private UICircle _loadingCircle;
        
        private CancellationTokenSource _cancellationTokenConnection;
        

        [Inject]
        private void Install(UIManager uiManager, ServerManager serverManager)
        {
            _uiManager = uiManager;
            _serverManager = serverManager;
        }

        private void OnEnable()
        {
            _createRoomButton.OnTap += OnCreateRoomClick;
            _joinRoomButton.OnTap += OnJoinRoomClick;
            
            _serverManager.SetSingleGame(false);
            ConnectServer();

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _createRoomButton.OnTap -= OnCreateRoomClick;
            _joinRoomButton.OnTap -= OnJoinRoomClick;
            
            _cancellationTokenConnection?.Cancel();
            
            if (_loadingCircle != null)
            {
                _uiManager.HideElement(_loadingCircle);
                _loadingCircle = null;
            }
            
            _createRoomPanel.SetActive(false);
            _joinRoomPanel.SetActive(false);
            
            _panel.SetActive(false);
            
            if(!PhotonNetwork.InRoom) _serverManager.Disconnect();
        }

        private void OnCreateRoomClick()
        {
            _createRoomPanel.SetActive(true);
        }

        private void OnJoinRoomClick()
        {
            _joinRoomPanel.SetActive(true);
        }

        private async void ConnectServer()
        {
            _cancellationTokenConnection?.Cancel();
            _cancellationTokenConnection = new CancellationTokenSource();
            try
            {
                _panel.SetActive(false);
                
                _loadingCircle = _uiManager.ShowElement(_loadingCircleContract, transform);

                await _serverManager.Connect(_cancellationTokenConnection.Token);

                _uiManager.HideElement(_loadingCircle);

                _loadingCircle = null;

                _panel.SetActive(true);
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}