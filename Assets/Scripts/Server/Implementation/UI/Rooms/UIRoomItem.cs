using System;
using Game.UI;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Game.Server.UI.Rooms
{
    public class UIRoomItem : UIButton
    {
        [SerializeField] private TMP_Text _nameTextField;
        [SerializeField] private TMP_Text _countMemberTextField;
        [SerializeField] private GameObject _inactiveMask;
        
        private RoomInfo _roomInfo;

        public RoomInfo RoomInfo => _roomInfo;

        public event Action<UIRoomItem> OnRoomSelected;
        
        public void Initialize(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;

            _nameTextField.text = roomInfo.Name;
            _countMemberTextField.text = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
            
            SetSelected(false);
        }

        protected override void OnClick()
        {
            base.OnClick();
            OnRoomSelected?.Invoke(this);
        }

        public void SetSelected(bool isSelected)
        {
            _inactiveMask.SetActive(!isSelected);
        }
    }
}