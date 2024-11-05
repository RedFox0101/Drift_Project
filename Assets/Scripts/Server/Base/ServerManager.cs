using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Game.Assets;
using Game.Loadings;
using Game.Save;
using Game.Server.Implementation;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.Server
{
    public class ServerManager : IInitializable, IConnectionCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IPunPrefabPool
    {
        private const string _saveID = nameof(ServerManager);
        private static RoomOptions _roomOptions = new(maxPlayers: 10);
        
        private ServerConfig _config;
        private SaveManager _saveManager;
        private LoadingManager _loadingManager;
        private AssetsManager _assetsManager;
        
        private List<RoomInfo> _rooms = new();
        
        private List<ServerResource> _serverResourcesPool = new();

        public List<ServerResource> ServerResourcesPool => _serverResourcesPool;
        
        public event Action<ServerResource> OnServerResourceAdd;
        public event Action<ServerResource> OnServerResourceRemove;
        
        public event Action OnConnect;
        public event Action OnConnectMaster;
        public event Action OnDisconnect;

        public event Action OnJoinLobby;
        public event Action OnLeaveLobby;
        
        public event Action<List<RoomInfo>> OnRoomListChanged;

        public event Action<bool> OnCreateRoom;
        public event Action<bool> OnJoinRoom;
        public event Action OnLeaveRoom;
        
        [Inject]
        private void Install(ServerConfig serverConfig, SaveManager saveManager, 
            LoadingManager loadingManager,  AssetsManager assetsManager) 
        {
            _config = serverConfig;
            _saveManager = saveManager;
            _loadingManager = loadingManager;
            _assetsManager = assetsManager;

        }
        
        public void Initialize()
        {
            PhotonNetwork.PrefabPool = this;
            
            PhotonNetwork.AddCallbackTarget(this);

            if (_saveManager.TryGetData(_saveID, out string nickName))
            {
                SetNickName(nickName);
            }
            else
            {
                SetNickName($"Player{Random.Range(1, 100)}");
            }
        }

        public GameObject InstantiatePlayer(Vector3 position, Quaternion rotation)
        {
            return InstantiateObject(_config.Character, position, rotation);
        }
        
        public GameObject InstantiateObject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object, IAsset
        {
            return PhotonNetwork.Instantiate(prefab.name, position, rotation);
        }
        
        public void SetNickName(string nickname)
        {
            PhotonNetwork.NickName = nickname;
            
            _saveManager.SetData(_saveID, nickname);
        }

        public string GetNickName()
        {
            return PhotonNetwork.NickName;
        }

        public bool IsMaster()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return true;
            
            return PhotonNetwork.IsMasterClient;
        }

        public void SetSingleGame(bool isSingle)
        {
            if (!PhotonNetwork.OfflineMode)
            {
                Disconnect();
            }
            
            PhotonNetwork.OfflineMode = isSingle;
        }

        public async UniTask<bool> Connect(CancellationToken cancellationToken = default)
        {
            if (PhotonNetwork.IsConnected) return true;

            bool isCompleted = false;

            Action actionWait = () => isCompleted = true;

            OnConnectMaster += actionWait;

            bool isSuccess = PhotonNetwork.ConnectUsingSettings();

            if (isSuccess) await UniTask.WaitWhile(() => !isCompleted, cancellationToken: cancellationToken);
            
            OnConnectMaster -= actionWait;
                
            if(isSuccess) isSuccess = await JoinLobby(cancellationToken);
            
            return isSuccess;
        }

        public async void Disconnect()
        {
            if (PhotonNetwork.InRoom) await LeaveRoom();

            if (PhotonNetwork.InLobby) await LeaveLobby();
            
            if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
        }

        private async UniTask<bool> JoinLobby(CancellationToken cancellationToken = default)
        {
            bool isCompleted = false;

            Action actionWait = () => isCompleted = true;

            OnJoinLobby += actionWait;

            bool isSuccess = PhotonNetwork.JoinLobby();

            if (isSuccess) await UniTask.WaitWhile(() => !isCompleted, cancellationToken: cancellationToken);

            OnJoinLobby -= actionWait;

            return isSuccess;
        }
        
        public async UniTask<bool> LeaveLobby()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InLobby) return false;
            
            bool isCompleted = false;

            Action actionWait = () => isCompleted = true;

            OnLeaveLobby += actionWait;

            bool isSuccess = PhotonNetwork.LeaveLobby();

            if (isSuccess) await UniTask.WaitWhile(() => !isCompleted);
            
            OnLeaveLobby -= actionWait;
            
            return isSuccess;
        }
        
        public List<RoomInfo> GetRooms()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InLobby) return null;
            
            // if (!PhotonNetwork.IsConnected && !await Connect()) return null;
            
            return _rooms;
        }

        public async UniTask<bool> CreateRoom(string loadingDataID, Action isSuccessCallback = null)
        {
            if (!PhotonNetwork.IsConnected && !await Connect()) return false;

            bool isSuccess = false;
            bool isCompleted = false;

            Action<bool> actionWait = new Action<bool>(result =>
            {
                isCompleted = true;
                isSuccess = result;

                if (isSuccess)
                {
                    SetRoomLoadingData(loadingDataID);

                    _loadingManager.Load(loadingDataID);

                    isSuccessCallback?.Invoke();
                }
            });
            
            OnCreateRoom += actionWait;
            
            isSuccess = PhotonNetwork.CreateRoom(PhotonNetwork.NickName, _roomOptions); 
            
            if (isSuccess) await UniTask.WaitWhile(() => !isCompleted);
            
            OnCreateRoom -= actionWait;
            
            return isSuccess;
        }

        public async UniTask<bool> JoinRoom(string roomName, Action isSuccessCallback = null)
        {
            if (!PhotonNetwork.IsConnected && !await Connect()) return false;
            
            bool isSuccess = false;
            bool isCompleted = false;

            Action<bool> actionWait = new Action<bool>(result =>
            {
                isCompleted = true;
                isSuccess = result;
                
                if (isSuccess)
                {
                    var loadingDataID = GetRoomLoadingData();

                    _loadingManager.Load(loadingDataID);

                    isSuccessCallback?.Invoke();
                }
            });
            
            OnJoinRoom += actionWait;
            
            isSuccess = PhotonNetwork.JoinRoom(roomName);
            
            if (isSuccess) await UniTask.WaitWhile(() => !isCompleted);
            
            OnJoinRoom -= actionWait;
            
            return isSuccess;
        }

        public async UniTask<bool> LeaveRoom()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return false;
            
            bool isCompleted = false;

            Action actionWait = () => isCompleted = true;

            OnLeaveRoom += actionWait;

            bool isSuccess = PhotonNetwork.LeaveRoom();

            if (isSuccess) await UniTask.WaitWhile(() => !isCompleted);
            
            OnLeaveRoom -= actionWait;
            
            return isSuccess;
        }

        public Hashtable GetRoomProperties()
        {
            if (!PhotonNetwork.InRoom) return null;
            
            return PhotonNetwork.CurrentRoom.CustomProperties;
        }

        public void SetRoomProperties(Hashtable properties)
        {
            if (!PhotonNetwork.InRoom) return;
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        
        private void SetRoomLoadingData(string loadingDataID)
        {
            if (!PhotonNetwork.InRoom) return;

            var properties = GetRoomProperties();

            if (properties == null) properties = new();

            if (properties.ContainsKey(nameof(LoadingDataScriptable)))
            {
                properties[nameof(LoadingDataScriptable)] = loadingDataID;
            }
            else properties.Add(nameof(LoadingDataScriptable), loadingDataID);
            
            SetRoomProperties(properties);
        }

        private string GetRoomLoadingData()
        {
            if (!PhotonNetwork.InRoom) return null;
            
            var properties = GetRoomProperties();

            return properties.TryGetValue(nameof(LoadingDataScriptable), out var loadingDataID) ? loadingDataID as string : null;
        }

        public void SetRoomOpen(bool isOpen)
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;

            PhotonNetwork.CurrentRoom.IsOpen = isOpen;
            PhotonNetwork.CurrentRoom.IsVisible = isOpen;
        }

        #region IConnectionCallbacks
        
        public void OnConnected()
        {
            OnConnect?.Invoke();
        }
        
        public void OnConnectedToMaster()
        {
            OnConnectMaster?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            _serverResourcesPool.Clear();
            OnDisconnect?.Invoke();
        }

        public void OnRegionListReceived(RegionHandler regionHandler) { }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

        public void OnCustomAuthenticationFailed(string debugMessage) { }

        #endregion

        #region ILobbyCallbacks

        
        public void OnJoinedLobby()
        {
            OnJoinLobby?.Invoke();
        }

        public void OnLeftLobby()
        {
            OnLeaveLobby?.Invoke();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _rooms = roomList;
            OnRoomListChanged?.Invoke(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
        
        #endregion

        #region IMatchmakingCallbacks
        
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnCreatedRoom()
        {
            OnCreateRoom?.Invoke(true);
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            OnCreateRoom?.Invoke(false);
        }

        public void OnJoinedRoom()
        {
            OnJoinRoom?.Invoke(true);
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            OnJoinRoom?.Invoke(false);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            OnJoinRoom?.Invoke(false);
        }

        public void OnLeftRoom()
        {
            _serverResourcesPool.Clear();
            OnLeaveRoom?.Invoke();
        }
        
        #endregion

        #region IPunPrefabPool

        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            var assetResource = _assetsManager.GetAssetResource<ServerResource>(prefabId, position, rotation, null);
            
            _serverResourcesPool.Add(assetResource);
            return assetResource?.Asset;
        }

        public void Destroy(GameObject gameObject)
        {
            _serverResourcesPool.Remove(gameObject.GetComponent<ServerResource>());
            Object.Destroy(gameObject);
        }
        
        #endregion
    }
}