using Game.Rewards;
using UnityEngine;

namespace Game.Shop.UI
{
    public struct ShopItemInfo
    {
        [field: SerializeField] public IRewardData Reward { get; private set; }
        [field: SerializeField] public IPriceData Price { get; private set; }
    }
}