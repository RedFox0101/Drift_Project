using System;
using System.Linq;
using Game.Shop.Datas;
using Sirenix.Utilities;
using Zenject;

namespace Game.Shop.Services
{
    public class ListPriceService : IShopService
    {
        private ShopManager _shopManager;
        
        public event Action<IPriceData> OnBuy;

        [Inject]
        private void Install(ShopManager shopManager)
        {
            _shopManager = shopManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(ListPriceData);
        }

        public bool CanBuy(IPriceData priceData)
        {
            if (priceData is ListPriceData listPrice)
            {
                return listPrice.Prices.All(_shopManager.CanBuy);   
            }

            return false;
        }

        public void Buy(IPriceData priceData)
        {
            if (priceData is ListPriceData listPrice)
            {
                listPrice.Prices.ForEach(price => _shopManager.TryBuy(price));
                OnBuy?.Invoke(priceData);
            }
        }

        public UIPrice GetUI(IPriceData priceData)
        {
            return default;
        }
    }
}