using UnityEngine;
using Zenject;

namespace Game.Settings.Installers
{
    public class SettingsManagerInstaller : MonoInstaller<SettingsManagerInstaller>
    {
        [SerializeField] private SettingsConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SettingsManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SettingsConfig>().FromInstance(_config).AsSingle();
        }
    }
}