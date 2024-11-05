using Game.Equipments.ItemData;
using UnityEngine;

namespace Game.Equipments.Implementation.Handlers
{
    public class EquipmentMaterialHandler : EquipmentHandler<MeshRenderer, Material>
    {
        protected override Material GetDefaultItem()
        {
            return _targetSlot.material;
        }

        protected override void EquipItem(EquipItemDataScriptable equipItemData, Material objectVisual)
        {
            _targetSlot.material = objectVisual;
        }

        protected override void ResetEquipment()
        {
            _targetSlot.material = _defaultItem;
        }
    }
}