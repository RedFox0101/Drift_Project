using System;

namespace Game.Rewards
{
    public interface IRewardService
    {
        public event Action<IRewardData> OnReward;
        
        public Type GetTypeData();
        
        public void Reward(IRewardData rewardData);
    }
}