using System.Collections.Generic;
using Game.Assets;
using Game.Inventory;
using Game.Items.Data;
using Game.UI;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Shop.UI
{
    public class UIShopCategoriesScreen : UIScreen
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Transform _targetContainer;
        [SerializeField] private UIShopItem _contractShopItem;

        private UIManager _uiManager;
        private InventoryManager _inventoryManager;
        
        private List<UIShopItem> _shopItemsPool = new();
        

        [Inject]
        private void Install(UIManager uiManager, InventoryManager inventoryManager)
        {
            _uiManager = uiManager;
            _inventoryManager = inventoryManager;
        }

        public void Initialize(ShopItemCategories shopItemCategories)
        {
            _label.text = shopItemCategories.Name;

            shopItemCategories.Items?.ForEach(item =>
            {
                bool isCreate = true;

                if (item.Reward is ItemData itemReward)
                {
                    isCreate = _inventoryManager.CanAddItem(itemReward.Value, itemReward.Count);
                }

                if (isCreate)
                {
                    var uiElement = _uiManager.ShowElement<UIShopItem>(_contractShopItem, _targetContainer);

                    uiElement.Initialize(item);

                    uiElement.OnReleased += OnReleaseItem;
                    
                    _shopItemsPool.Add(uiElement);
                }
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _shopItemsPool.ForEach(item => _uiManager.HideElement(item));
            _shopItemsPool.Clear();
        }

        private void OnReleaseItem(IAsset asset)
        {
            asset.OnReleased -= OnReleaseItem;
            
            _shopItemsPool.Remove(asset as UIShopItem);
        }
    }
}