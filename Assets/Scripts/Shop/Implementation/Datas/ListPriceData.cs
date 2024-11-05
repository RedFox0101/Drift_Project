using UnityEngine;

namespace Game.Shop.Datas
{
    public class ListPriceData : IPriceData
    {
        [field: SerializeField] public IPriceData[] Prices { get; private set; }
    }
}