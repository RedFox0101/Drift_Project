using System.Collections.Generic;
using Game.UI;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Game.Server.UI.Rooms
{
    public class UIRoomPanel : UIElement
    {
        [SerializeField] private UIRoomItem _uiRoomItemContarct;
        [SerializeField] private Transform _containerRoom;
        
        [SerializeField] private UIButton _uiButton;
        
        private UIManager _uiManager;
        private ServerManager _serverManager;

        private UIRoomItem _currentRoomSelected;        
        private List<UIRoomItem> _uiRoomItemPool = new();

        [Inject]
        private void Install(UIManager uiManager, ServerManager serverManager)
        {
            _uiManager = uiManager;
            _serverManager = serverManager;
        }

        private void OnEnable()
        {
            _uiButton.OnTap += OnJoinClick;
            _serverManager.OnRoomListChanged += OnRoomListChanged;
            
            OnRoomSelected(null);
            
            OnRoomListChanged(_serverManager.GetRooms());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _uiButton.OnTap -= OnJoinClick;
            _serverManager.OnRoomListChanged += OnRoomListChanged;
            
            ClearRoomList();
            
            _currentRoomSelected = null;
            
            _uiButton.gameObject.SetActive(false);
        }

        private void OnJoinClick()
        {
            if (_currentRoomSelected == null || _currentRoomSelected.RoomInfo == null) return;

            _serverManager.JoinRoom(_currentRoomSelected.RoomInfo.Name);
        }

        private void ClearRoomList()
        {
            _uiRoomItemPool?.ForEach(uiRoomItem =>
            {
                uiRoomItem.OnRoomSelected -= OnRoomSelected;
                _uiManager.HideElement(uiRoomItem);
            });
            _uiRoomItemPool.Clear();
        }

        private void OnRoomListChanged(List<RoomInfo> roomInfos)
        {
            ClearRoomList();
            roomInfos?.ForEach(room =>
            {
                var uiRoomItem = _uiManager.ShowElement(_uiRoomItemContarct, _containerRoom);
                _uiRoomItemPool.Add(uiRoomItem);
                
                uiRoomItem.Initialize(room);
                
                uiRoomItem.OnRoomSelected += OnRoomSelected;
            });
        }

        private void OnRoomSelected(UIRoomItem uiRoomItem)
        {
            _uiRoomItemPool?.ForEach(room => uiRoomItem.SetSelected(false));
            
            uiRoomItem?.SetSelected(true);

            _currentRoomSelected = uiRoomItem;
            
            _uiButton.gameObject.SetActive(uiRoomItem != null);
        }
    }
}