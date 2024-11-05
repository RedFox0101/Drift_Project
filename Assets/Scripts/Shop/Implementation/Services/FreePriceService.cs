using System;
using Game.Shop.Datas;

namespace Game.Shop.Services
{
    public class FreePriceService : IShopService
    {
        public event Action<IPriceData> OnBuy;
        
        public Type GetTypeData()
        {
            return typeof(FreePriceData);
        }

        public bool CanBuy(IPriceData priceData)
        {
            return priceData is FreePriceData; 
        }

        public void Buy(IPriceData priceData)
        {
            OnBuy?.Invoke(priceData);
        }

        public UIPrice GetUI(IPriceData priceData)
        {
            return new UIPrice()
            {
                Count = "FREE"
            };
        }
    }
}