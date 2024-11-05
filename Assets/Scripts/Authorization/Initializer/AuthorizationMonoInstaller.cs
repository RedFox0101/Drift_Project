using Game.Loadings;
using Zenject;

namespace Game
{
    public class AuthorizationMonoInstaller : MonoInstaller<AuthorizationMonoInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AuthorizationService>().AsSingle();
        }
    }
}
