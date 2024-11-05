using Zenject;

namespace Game.Predicates.Installers
{
    public class PredicateManagerInstaller : MonoInstaller<PredicateManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PredicateManager>().AsSingle().NonLazy();
        }
    }
}