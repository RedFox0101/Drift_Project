using UnityEngine;
using Zenject;

namespace Game.Levels.Installers
{
    public class LevelsManagerInstaller : MonoInstaller<LevelsManagerInstaller>
    {
        [SerializeField] private LevelsConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelsManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelsConfig>().FromInstance(_config).AsSingle();
        }
    }
}