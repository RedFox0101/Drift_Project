using System;
using Game.Items.Data;
using Game.Predicates;
using Zenject;

namespace Game.Inventory.Predicates
{
    public class ItemPredicateData : IPredicateService
    {
        private InventoryManager _inventoryManager;

        [Inject]
        private void Install(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(ItemData);
        }

        public bool Check(IPredicateData predicateData)
        {
            if (predicateData is ItemData itemData)
            {
                return _inventoryManager.ContainsItem(itemData.Value, itemData.Count);
            }

            return false;
        }
    }
}