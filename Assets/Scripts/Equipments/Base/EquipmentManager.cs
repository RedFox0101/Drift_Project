using System;
using System.Collections.Generic;
using Game.Data;
using Game.Equipments.ItemData;
using Game.Items;
using Game.Save;
using Sirenix.Utilities;
using Zenject;

namespace Game.Equipments
{
    public class EquipmentManager : DataManager<EquipmentDataScriptable, EquipmentConfig>
    {
        private const string _saveID = nameof(EquipmentManager);
        
        private SaveManager _saveManager;
        private ItemsManager _itemsManager;

        private Dictionary<string, string> _currentEquipment = new();
        
        public event Action<string, string> OnEquipItem;
        public event Action<string> OnDeEquipItem;
        public event Action OnEquipsItemChanged;
        
        [Inject]
        private void Install(SaveManager saveManager, ItemsManager itemsManager)
        {
            _saveManager = saveManager;
            _itemsManager = itemsManager;
        }
        
        protected override void Initialized()
        {
            base.Initialized();
            
            if(_saveManager.TryGetData(_saveID, out Dictionary<string, string> equipment))
            {
                _currentEquipment = equipment;
            }
        }

        public void Equip(string itemID)
        {
            if (itemID.IsNullOrWhitespace()) return;

            var itemData = _itemsManager.GetData(itemID) as EquipItemDataScriptable;

            if (itemData != null)
            {
                _currentEquipment.TryAdd(itemData.EquipmentSlotID, "");

                _currentEquipment[itemData.EquipmentSlotID] = itemID;
                
                OnEquipItem?.Invoke(itemData.EquipmentSlotID, itemID);
                OnEquipsItemChanged?.Invoke();

                _saveManager.SetData(_saveID, _currentEquipment);
            }
        }

        public void DeEquip(string equipSlot)
        {
            if (equipSlot.IsNullOrWhitespace()) return;

            if (_currentEquipment.ContainsKey(equipSlot))
            {
                _currentEquipment.Remove(equipSlot);
                
                OnDeEquipItem?.Invoke(equipSlot);
                OnEquipsItemChanged?.Invoke();
                
                _saveManager.SetData(_saveID, _currentEquipment);
            }
        }

        public string GetSlotItem(string equipmentID)
        {
            return _currentEquipment.TryGetValue(equipmentID, out var itemID) ? itemID : null;
        }
        
        public Dictionary<string, string> GetEquipment()
        {
            return _currentEquipment;
        }
    }
}