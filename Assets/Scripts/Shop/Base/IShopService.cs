using System;

namespace Game.Shop
{
    public interface IShopService
    {
        public event Action<IPriceData> OnBuy;
        
        public Type GetTypeData();

        public bool CanBuy(IPriceData priceData);
        
        public void Buy(IPriceData priceData);

        public UIPrice GetUI(IPriceData priceData);
    }
}