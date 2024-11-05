using System.Collections.Generic;
using Game.Equipments.ItemData;
using Game.Items;
using Game.UI;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Equipments.UI
{
    public class UICustomizationSlotScreen : UIScreen
    {
        [SerializeField] private UICustomizationItem _customizationItemContract;
        [SerializeField] private Transform _containerItems;
        [SerializeField] private TMP_Text _labelTextField;
        
        private UIManager _uiManager;
        private ItemsManager _itemsManager;
        
        private EquipmentDataScriptable _equipmentData;
        private List<UICustomizationItem> _customizationItemsPool = new();
        
        
        [Inject]
        private void Install(UIManager uiManager, ItemsManager itemsManager)
        {
            _uiManager = uiManager;
            _itemsManager = itemsManager;
        }
        
        public void Initialize(EquipmentDataScriptable equipmentData)
        {
            _equipmentData = equipmentData;
            
            if (_equipmentData != null)
            {
                _labelTextField.text = equipmentData.Name;

                var itemDatas = _itemsManager.GetDataAll();

                itemDatas.ForEach(itemData =>
                {
                    if (!itemData.IsSystem && itemData is EquipItemDataScriptable equipItemDataScriptable)
                    {
                        if (equipItemDataScriptable.EquipmentSlotID.Equals(equipmentData.ID))
                        {
                            var asset = _uiManager.ShowElement(_customizationItemContract, _containerItems);
                            
                            asset.Initialize(equipItemDataScriptable);
                            
                            _customizationItemsPool.Add(asset);
                        }
                    }
                });
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _customizationItemsPool.ForEach(item => _uiManager.HideElement(item));
            _customizationItemsPool.Clear();
        }
    }
}