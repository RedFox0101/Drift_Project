using UnityEngine;
using Zenject;

namespace Game.Audios.Installers
{
    public class AudioManagerInstaller : MonoInstaller<AudioManagerInstaller>
    {
        [SerializeField] private AudioConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AudioConfig>().FromInstance(_config).AsSingle();
        }
    }
}