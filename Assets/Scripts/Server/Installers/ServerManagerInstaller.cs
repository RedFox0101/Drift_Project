using UnityEngine;
using Zenject;

namespace Game.Server.Installers
{
    public class ServerManagerInstaller : MonoInstaller<ServerManagerInstaller>
    {
        [SerializeField] private ServerConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ServerManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ServerConfig>().FromInstance(_config).AsSingle();
        }
    }
}