using System;
using Game.Items;
using Game.Items.Data;
using Game.Shop;
using Zenject;

namespace Game.Inventory.Shops
{
    public class ItemShopService : IShopService
    {
        private ItemsManager _itemsManager;
        private InventoryManager _inventoryManager;
        
        public event Action<IPriceData> OnBuy;

        [Inject]
        private void Install(ItemsManager itemsManager, InventoryManager inventoryManager)
        {
            _itemsManager = itemsManager;
            _inventoryManager = inventoryManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(ItemData);
        }

        public bool CanBuy(IPriceData priceData)
        {
            if (priceData is ItemData itemData)
            {
                return _inventoryManager.ContainsItem(itemData.Value, itemData.Count);
            }
            
            return false;
        }

        public void Buy(IPriceData priceData)
        {
            if (priceData is ItemData itemData)
            {
                _inventoryManager.Remove(itemData.Value, itemData.Count);  
                OnBuy?.Invoke(priceData);
            }
        }

        public UIPrice GetUI(IPriceData priceData)
        {
            if (priceData is ItemData itemData)
            {
                var item = _itemsManager.GetData(itemData.Value);
                if (itemData != null)
                {
                    return new UIPrice()
                    {
                        Count = itemData.Count >= 1000 ? $"{itemData.Count / 1000f}k" : itemData.Count.ToString(),
                        Icon = item.Icon
                    };
                }
            }

            return default;
        }
    }
}