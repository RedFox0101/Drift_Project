using Zenject;

namespace Game.Shop.Installers
{
    public class ShopManagerInstaller : MonoInstaller<ShopManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShopManager>().AsSingle().NonLazy();
        }
    }
}