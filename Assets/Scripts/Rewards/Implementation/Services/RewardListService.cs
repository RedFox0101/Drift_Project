using System;
using Game.Rewards.Data;
using Zenject;

namespace Game.Rewards.Services
{
    public class RewardListService : IRewardService
    {
        public event Action<IRewardData> OnReward;

        private RewardsManager _rewardsManager;

        [Inject]
        private void Install(RewardsManager rewardsManager)
        {
            _rewardsManager = rewardsManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(RewardListData);
        }

        public void Reward(IRewardData rewardData)
        {
            if (rewardData is RewardListData rewardsData)
            {
                rewardsData.Value?.ForEach(reward => _rewardsManager.Reward(reward));
            }
        }
    }
}