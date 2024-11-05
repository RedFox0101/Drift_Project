using UnityEngine;
using Zenject;

namespace Game.Unlocks.Installers
{
    public class UnlockManagerInstaller : MonoInstaller<UnlockManagerInstaller>
    {
        [SerializeField] private UnlockConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UnlockManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UnlockConfig>().FromInstance(_config).AsSingle();
        }
    }
}