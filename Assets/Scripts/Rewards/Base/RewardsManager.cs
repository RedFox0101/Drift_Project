using System;
using System.Collections.Generic;
using System.Linq;
using Game.Inventory.Rewards;
using Game.Rewards.Services;
using Game.Unlocks.Rewards;
using Sirenix.Utilities;
using Zenject;

namespace Game.Rewards
{
    public class RewardsManager : IInitializable
    {
        private static readonly IRewardService[] _serviceInstance = {
            new ItemRewardService(),
            new RewardListService(),
            new UnlockRewardService()
        };

        private Dictionary<Type, IRewardService> _rewardServices = new();

        private bool _isInitialize;
        
        public event Action<IRewardData> OnReward;

        [Inject]
        private void Install(DiContainer diContainer)
        {
            _serviceInstance.ForEach(diContainer.Inject);
        }

        public void Initialize()
        {
            if (_isInitialize) return;
            
            _isInitialize = true;
            
            _rewardServices = _serviceInstance.ToDictionary(key => key.GetTypeData(), value => value);

            _serviceInstance.ForEach(service => service.OnReward += OnReward);
        }

        public void Reward(IRewardData rewardData)
        {
            if (rewardData == null) return;
            
            if(!_isInitialize) Initialize();

            if (_rewardServices.TryGetValue(rewardData.GetType(), out var service))
            {
                service.Reward(rewardData);
            }
        }
    }
}