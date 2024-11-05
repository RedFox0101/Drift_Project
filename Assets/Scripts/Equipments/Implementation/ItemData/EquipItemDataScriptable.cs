using Game.Data.Attributes;
using Game.Items;
using UnityEngine;

namespace Game.Equipments.ItemData
{
    [CreateAssetMenu(menuName = "Data/Items/Equipment Item Data", fileName = "Equipment Item Data")]
    public class EquipItemDataScriptable : ItemDataScriptable
    {
        [field: SerializeField, DataID(typeof(EquipmentConfig))] public string EquipmentSlotID { get; private set; }
        [field: SerializeField] public Object ObjectVisual { get; private set; }
    }
}