using System;
using Game.Rewards;
using Game.Unlocks.Data;
using Zenject;

namespace Game.Unlocks.Rewards
{
    public class UnlockRewardService : IRewardService
    {
        public event Action<IRewardData> OnReward;

        private UnlockManager _unlockManager;

        [Inject]
        private void Install(UnlockManager unlockManager)
        {
            _unlockManager = unlockManager;
        }

        public Type GetTypeData()
        {
            return typeof(UnlockData);
        }

        public void Reward(IRewardData rewardData)
        {
            if (rewardData is UnlockData unlockData)
            {
                _unlockManager.Unlock(unlockData.Value);
                OnReward?.Invoke(rewardData);
            }
        }
    }
}