using Game.Data;
using UnityEngine;

namespace Game.Equipments
{
    [CreateAssetMenu(menuName = "Data/Equipment/Equipment Data", fileName = "Equipment Data")]
    public class EquipmentDataScriptable : DataScriptable
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}