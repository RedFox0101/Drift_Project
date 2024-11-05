using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Loadings.Boot
{
    public class BootLoader : MonoBehaviour
    {
        private LoadingManager _loadingManager;
        
        [Inject]
        private void Install(LoadingManager loadingManager)
        {
            _loadingManager = loadingManager;
        }

        private void Start()
        {
            _loadingManager.LoadMeta();
        }
    }
}