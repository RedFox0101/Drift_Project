using System.Collections.Generic;
using Game.Characters.Character.Car;
using Game.Characters.Datas;
using Game.Equipments.Datas;
using Game.Inventory;
using Game.Levels.Core;
using Game.Server;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Characters.Character
{
    public class PlayerController : CarController, IInventoryOwner, IServerController
    {
        public const string PlayerID = "Player";

        [SerializeField] private PhotonView _photonView;

        private ScorePoints _scorePoints;
        private EquipmentsData _equipmentsData;

        private LevelController _levelController;
        
        public PhotonView PhotonView => _photonView;
        public string InventoryOwnerID => PlayerID;

        public void InitializeServer()
        {
            var skidmarks = FindObjectOfType<Skidmarks>();
            WheelController.ForEach(wheel => wheel.WheelSkid.SetController(skidmarks));
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            _scorePoints = GetDataField<ScorePoints>();
            _equipmentsData = GetDataField<EquipmentsData>();
            _rigidbody.isKinematic = !_photonView.IsMine;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _velocityData.OnChanged += OnVelocityChanged;
            _directionData.OnChanged += OnDirectionChanged;
            _scorePoints.OnChanged += OnScorePointsChanged;
            _equipmentsData.OnChanged += OnEquipmentsChanged;
            
            OnEquipmentsChanged(_equipmentsData.Value);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _velocityData.OnChanged -= OnVelocityChanged;
            _directionData.OnChanged -= OnDirectionChanged;
            _scorePoints.OnChanged -= OnScorePointsChanged;
            _equipmentsData.OnChanged -= OnEquipmentsChanged;
        }

        private void OnDirectionChanged(Vector2Int direction)
        {
            if (_photonView.IsMine)
            {
                _photonView.RPC(nameof(SetRPCDirection), RpcTarget.Others, direction.x, direction.y, _photonView.Owner);   
            }
        }
        
        [PunRPC]
        private void SetRPCDirection(int x, int y, Player player)
        {
            if (_photonView.Owner.Equals(player))
            {
                _directionData.SetValue(new Vector2Int(x, y));
            }
        }
        
        private void OnVelocityChanged(float velocity)
        {
            if (_photonView.IsMine)
            {
                _photonView.RPC(nameof(SetRPCVelocity), RpcTarget.Others, velocity, _photonView.Owner);
            }
        }
        
        [PunRPC]
        private void SetRPCVelocity(float velocity, Player player)
        {
            if(_photonView.Owner.Equals(player))
            {
                _velocityData.SetValue(velocity);
            }
        }
        
        private void OnScorePointsChanged(int points)
        {
            if (_photonView.IsMine)
            {
                _photonView.RPC(nameof(SetRPCDriftPoints), RpcTarget.OthersBuffered, points, _photonView.Owner);
            }
        }
        
        [PunRPC]
        private void SetRPCDriftPoints(int points, Player player)
        {
            if(_photonView.Owner.Equals(player))
            {
                _scorePoints.SetValue(points);
            }
        }

        public override void SetDriving(bool isDriving)
        {
            if (isDriving == IsDriving) return;

            _photonView.RPC(nameof(SetRPCDriving), RpcTarget.OthersBuffered, isDriving);
            
            base.SetDriving(isDriving);
        }

        [PunRPC]
        public void SetRPCDriving(bool isDriving)
        {
            _levelController?.SetActiveLevel(isDriving);
        }

        public void SetLevelController(LevelController levelController)
        {
            _levelController = levelController;
        }

        private void OnEquipmentsChanged(List<string> list)
        {
            if (_photonView.IsMine)
            {
                list ??= new List<string>();
                
                _photonView.RPC(nameof(SetEquipmentsVelocity), RpcTarget.OthersBuffered,  list.ToArray(), _photonView.Owner);
            }
        }
        
        [PunRPC]
        private void SetEquipmentsVelocity(string[] equipmentsList, Player player)
        {
            if(_photonView.Owner.Equals(player))
            {
                _equipmentsData.Clear();
                equipmentsList?.ForEach(item => _equipmentsData.Add(item));
            }
        }
    }
}