using UnityEngine;
using Zenject;

namespace Game.Equipments.Installers
{
    public class EquipmentManagerInstaller : MonoInstaller<EquipmentManagerInstaller>
    {
        [SerializeField] private EquipmentConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EquipmentManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EquipmentConfig>().FromInstance(_config).AsSingle();
        }
    }
}