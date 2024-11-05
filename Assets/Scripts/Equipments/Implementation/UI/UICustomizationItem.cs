using Game.Equipments.ItemData;
using Game.Inventory;
using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Equipments.UI
{
    public class UICustomizationItem : UIButton
    {
        [SerializeField] private TMP_Text _labelTextField;
        [SerializeField] private GameObject _maskActive;
        [SerializeField] private GameObject _maskLock;

        private EquipmentManager _equipmentManager;
        private InventoryManager _inventoryManager;
        private EquipItemDataScriptable _itemData;
        
        [Inject]
        private void Install(EquipmentManager equipmentManager, InventoryManager inventoryManager)
        {
            _equipmentManager = equipmentManager;
            _inventoryManager = inventoryManager;
        }
        
        public void Initialize(EquipItemDataScriptable itemData)
        {
            _itemData = itemData;
            
            if (itemData != null)
            {
                _labelTextField.text = itemData.Name;
                _maskLock.SetActive(!_inventoryManager.ContainsItem(itemData.ID));

                OnEquipItem(itemData.EquipmentSlotID, _equipmentManager.GetSlotItem(itemData.EquipmentSlotID));
                
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _equipmentManager.OnEquipItem += OnEquipItem;
            _equipmentManager.OnDeEquipItem += OnDeEquipItem;
            _inventoryManager.OnItemAdded += OnItemAdd;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _equipmentManager.OnEquipItem -= OnEquipItem;
            _equipmentManager.OnDeEquipItem -= OnDeEquipItem;
            _inventoryManager.OnItemAdded -= OnItemAdd;
        }

        private void OnEquipItem(string slotID, string itemID)
        {
            if (_itemData.EquipmentSlotID.Equals(slotID))
            {
                _maskActive.SetActive(itemID != null && itemID.Equals(_itemData.ID));
            }
        }
        

        private void OnDeEquipItem(string slotID)
        {
            if (_itemData.EquipmentSlotID.Equals(slotID))
            {
                _maskActive.SetActive(false);
            }
        }

        private void OnItemAdd(IInventoryOwner arg1, string itemID, int count)
        {
            if (itemID.Equals(_itemData.ID))
            {
                _maskLock.SetActive(false);
            }
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (_inventoryManager.ContainsItem(_itemData.ID))
            {
                var currentEquipItem = _equipmentManager.GetSlotItem(_itemData.EquipmentSlotID);

                if (currentEquipItem != null && currentEquipItem.Equals(_itemData.ID))
                {
                    _equipmentManager.DeEquip(_itemData.EquipmentSlotID);
                }
                else
                {
                    _equipmentManager.Equip(_itemData.ID);
                }  
            }
        }
    }
}