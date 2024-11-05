using Game.Characters.Character;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Characters.Handlers.Player
{
    public class PlayerServerHandler : IHandlers<PlayerController>
    {
        [SerializeField] private GameObject[] _playerObjects;

        private void Awake()
        {
            _playerObjects?.ForEach(obj => obj.SetActive(_targetData.PhotonView.IsMine));
        }
    }
}