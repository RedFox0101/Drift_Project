using UnityEngine;
using Zenject;

namespace Game.Loadings.Boot
{
    public class BootLoader : MonoBehaviour
    {
        private LoadingService _loadingService;
        
        [Inject]
        private void Install(LoadingService loadingManager)
        {
            _loadingService = loadingManager;
        }

        private void Start()
        {
            _loadingService.LoadAuthorization();
        }
    }
}