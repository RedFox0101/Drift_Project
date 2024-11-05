using UnityEngine;
using Zenject;

namespace Game.Loadings.Installers
{
    public class LoadingManagerInstaller : MonoInstaller<LoadingManagerInstaller>
    {
        [SerializeField] private LoadingConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LoadingManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadingConfig>().FromInstance(_config).AsSingle();
        }
    }
}