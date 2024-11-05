using Zenject;

namespace Game
{
    public class UserMonoInstaller : MonoInstaller<UserMonoInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UserService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseDatabaseService>().AsSingle();
        }
    }
}
