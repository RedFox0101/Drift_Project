using UnityEngine;

namespace Game.Shop.UI
{
    public struct ShopItemCategories
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ShopItemInfo[] Items { get; private set; }
    }
}