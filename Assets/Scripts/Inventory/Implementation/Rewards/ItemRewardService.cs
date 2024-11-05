using System;
using Game.Items.Data;
using Game.Rewards;
using Zenject;

namespace Game.Inventory.Rewards
{
    public class ItemRewardService : IRewardService
    {
        public event Action<IRewardData> OnReward;
        
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

        public void Reward(IRewardData rewardData)
        {
            if (rewardData is ItemData itemData)
            {
                _inventoryManager.Add(itemData.Value, itemData.Count);
                OnReward?.Invoke(rewardData);
            }
        }
    }
}