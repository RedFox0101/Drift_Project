using System.Linq;
using Game.Characters;
using Game.Characters.Character;
using Game.Equipments.Datas;
using Sirenix.Utilities;
using Zenject;

namespace Game.Equipments.Implementation.Handlers
{
    public class PlayerEquipmentHandler : IHandlers<PlayerController>
    {
        private EquipmentsData _equipmentsData;
        private EquipmentManager _equipmentManager;
        
        [Inject]
        private void Install(EquipmentManager equipmentManager)
        {
            _equipmentManager = equipmentManager;
        }
        
        private void Awake()
        {
            _equipmentsData = _targetData.GetDataField<EquipmentsData>();
        }

        private void OnEnable()
        {
            _equipmentManager.OnEquipsItemChanged += OnEquipsItemChanged;
            OnEquipsItemChanged();
        }

        private void OnDisable()
        {
            _equipmentManager.OnEquipsItemChanged -= OnEquipsItemChanged;
        }

        private void OnEquipsItemChanged()
        {
            var items = _equipmentManager.GetEquipment()?.Select(item => item.Value);
            
            _equipmentsData.Clear();
            
            items?.ForEach(item => _equipmentsData.Add(item));
        }
    }
}