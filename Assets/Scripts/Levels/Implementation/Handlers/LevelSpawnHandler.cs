using System.Collections.Generic;
using System.Linq;
using Game.Characters;
using Game.Characters.Character;
using Game.Levels.Core;
using Game.Server;
using Game.Server.Implementation;
using UnityEngine;
using Zenject;

namespace Game.Levels.Handlers
{
    public class LevelSpawnHandler : IHandlers<LevelController>
    {
        private const string _serverSpawnPoint = "SpawnPoint";
        private Dictionary<ServerResource, PlayerController> _players;
        private Queue<Transform> _spawnPointsQueue = new();
        
        private ServerManager _serverManager;
        
        [Inject]
        private void Install(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }
        
        private void Awake()
        {
            if (_serverManager.IsMaster())
            {
                var roomProperties = _serverManager.GetRoomProperties();

                var spawnPositions = GetSpawnPoints();

                roomProperties.Add(_serverSpawnPoint, spawnPositions);

                _serverManager.SetRoomProperties(roomProperties);
            }

            Spawn();
        }

        private void Spawn()
        {
            var roomProperties = _serverManager.GetRoomProperties();
            
            Vector3[] spawnPoints = null;

            if (roomProperties.TryGetValue(_serverSpawnPoint, out var obj) && obj is Vector3[] serverSpawnPoints)
            {
                spawnPoints = serverSpawnPoints;
            }
            else spawnPoints = GetSpawnPoints();
            
            Vector3 spawnPoint = Vector3.zero;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints.FirstOrDefault();
                var newSpawnPoints = spawnPoints.ToList();
                newSpawnPoints.RemoveAt(0);
                spawnPoints = newSpawnPoints.ToArray();
            }
            else
            {
                spawnPoint = _targetData.transform.position;
            }
            
            _targetData.SpawnPlayer(spawnPoint, _targetData.transform.rotation);
            
            roomProperties[_serverSpawnPoint] = spawnPoints;
            
            _serverManager.SetRoomProperties(roomProperties);
        }

        private Vector3[] GetSpawnPoints()
        {
            return _targetData.SpawnPoints.Select(point => point.position).ToArray();
        }
    }
}