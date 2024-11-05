using Zenject;

namespace Game.Save.Installers
{
    public class SaveManagerInstaller : MonoInstaller<SaveManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
        }
    }
}