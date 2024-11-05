using System;
using Game.Characters.Character;
using Game.Server;
using UnityEngine;
using Zenject;

namespace Game.Levels.Core
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        
        private ServerManager _serverManager;
        private string _levelDataID;
        
        public Transform[] SpawnPoints => _spawnPoints;

        private PlayerController _player;

        public PlayerController Player => _player;
        
        public string LevelDataID => _levelDataID;

        public event Action<bool> OnLevelActiveChanged;
        public event Action OnLevelFinish;
        
        [Inject]
        private void Install(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        private void Awake()
        {
            InitLevelDataID();
        }

        private void InitLevelDataID()
        {
            var properties = _serverManager.GetRoomProperties();
            if(properties.TryGetValue(nameof(LevelDataScriptable), out var obj) && obj is string levelDataID)
            {
                _levelDataID = levelDataID;
            }
        }

        public void SpawnPlayer(Vector3 position, Quaternion rotation)
        {
            var asset = _serverManager.InstantiatePlayer(position, rotation);

            _player = asset.GetComponentInChildren<PlayerController>();
            
            _player.SetLevelController(this);
        }

        public void SetActiveLevel(bool isActive)
        {
            if (_serverManager.IsMaster())
            {
                _serverManager.ServerResourcesPool.ForEach(serverResource =>
                {
                    var resource = serverResource.GetComponentInChildren<PlayerController>();
                    if (resource != null)
                    {
                        resource.SetDriving(isActive);
                    }
                });
                if(isActive) _serverManager.SetRoomOpen(false);   
            }
            else _player.SetDriving(isActive);
            
            OnLevelActiveChanged?.Invoke(isActive);
        }
        
        
        public void LevelFinish()
        {
            OnLevelFinish?.Invoke();
        }
    }
}