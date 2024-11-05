using Cysharp.Threading.Tasks;
using Game.Assets.Assets;
using Game.Loadings;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Server.Implementation
{
    public class ServerResource : AssetResource
    {
        private LoadingManager _loadingManager;
        private ServerManager _serverManager;

        [Inject]
        private void Inject(LoadingManager loadingManager, ServerManager serverManager)
        {
            _serverManager = serverManager;
            _loadingManager = loadingManager;
        }
        
        protected void Awake()
        {
            WaitLoadings();
        }
        
        private async void WaitLoadings()
        {
            DontDestroyOnLoad(gameObject);

            await UniTask.WaitWhile(()=> _loadingManager.IsLoading);
            
            var scene = SceneManager.GetActiveScene();
            
            SceneManager.MoveGameObjectToScene(gameObject, scene);

            var serverController = GetComponentInChildren<IServerController>();

            serverController?.InitializeServer();
        }
    }
}