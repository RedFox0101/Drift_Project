using Game.Characters;
using Game.Characters.Datas;
using Game.Items.Data;
using Game.Levels.Core;
using Game.Levels.UI;
using Game.Rewards;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Levels.Handlers
{
    public class LevelFinishedHandler : IHandlers<LevelController>
    {
        [SerializeField] private UILevelWinScreen _winScreenContract;

        private UIManager _uiManager;
        private LevelsManager _levelsManager;
        private RewardsManager _rewardsManager;

        [Inject]
        private void Install(UIManager uiManager, LevelsManager levelsManager, RewardsManager rewardsManager)
        {
            _uiManager = uiManager;
            _levelsManager = levelsManager;
            _rewardsManager = rewardsManager;
        }
        
        private void OnEnable()
        {
            _targetData.OnLevelFinish += OnLevelFinish;
        }

        private void OnDisable()
        {
            _targetData.OnLevelFinish -= OnLevelFinish;
        }

        private void OnLevelFinish()
        {
            _targetData.SetActiveLevel(false);
            
            var levelData = _levelsManager.GetData(_targetData.LevelDataID);
            if (levelData != null)
            {
                var rewardData = new ItemData();
                if (levelData.Reward != null)
                {
                    var driftPoints = _targetData.Player.GetDataField<ScorePoints>();
                    
                    rewardData.SetInstance(levelData.Reward);
                    rewardData.SetCount(rewardData.Count * driftPoints.Value);
                }
                
                _rewardsManager.Reward(rewardData);
                _rewardsManager.Reward(levelData.RewardSystem);
                
                var asset = _uiManager.ShowElement(_winScreenContract);
                asset.Initialize(rewardData);
            }
        }
    }
}