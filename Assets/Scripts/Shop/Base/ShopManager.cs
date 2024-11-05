using System;
using System.Collections.Generic;
using System.Linq;
using Game.Inventory;
using Game.Inventory.Shops;
using Game.Shop.Services;
using Sirenix.Utilities;
using Zenject;

namespace Game.Shop
{
    public class ShopManager
    {
        private static readonly IShopService[] _serviceInstance = {
            new FreePriceService(),
            new ListPriceService(),
            new ItemShopService(),
        };

        private Dictionary<Type, IShopService> _shopServices = new();
        
        private InventoryManager _inventoryManager;

        private bool _isInitialize;
        
        public event Action<IPriceData> OnBuy;

        [Inject]
        private void Install(DiContainer diContainer)
        {
            _serviceInstance.ForEach(diContainer.Inject);
        }
        
        public void Initialize()
        {
            if (_isInitialize) return;
            
            _isInitialize = true;
            
            _shopServices = _serviceInstance.ToDictionary(key => key.GetTypeData(), value => value);

            _serviceInstance.ForEach(service => service.OnBuy += OnBuy);
        }

        public bool CanBuy(IPriceData priceData)
        {
            if(!_isInitialize) Initialize();
            
            if (_shopServices.TryGetValue(priceData.GetType(), out var service))
            {
                return service.CanBuy(priceData);
            }

            return false;
        }

        public bool TryBuy(IPriceData priceData)
        {
            if(!_isInitialize) Initialize();

            if (_shopServices.TryGetValue(priceData.GetType(), out var service))
            {
                if (CanBuy(priceData))
                {
                    service.Buy(priceData);
                    return true;
                }
            }
            
            return false;
        }

        public UIPrice GetUIPrice(IPriceData priceData)
        {
            if(!_isInitialize) Initialize();
            
            if (_shopServices.TryGetValue(priceData.GetType(), out var service))
            {
                return service.GetUI(priceData);
            }
            
            return default;
        }
    }
}