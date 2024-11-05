using Game.Data;
using Game.Data.Attributes;
using Game.Items.Data;
using Game.Loadings;
using Game.Predicates;
using Game.Rewards;
using UnityEngine;

namespace Game.Levels
{
    [CreateAssetMenu(menuName = "Data/Levels/Level Data", fileName = "Level Data")]
    public class LevelDataScriptable : DataScriptable
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, DataID(typeof(LoadingConfig))] public string LoadingDataID { get; private set; }
        [field: SerializeField] public IPredicateData Predicate { get; private set; }
        [field: SerializeField] public float DurationTimer { get; private set; }
        [field: SerializeField] public ItemData Reward { get; private set; }
        [field: SerializeField] public IRewardData RewardSystem { get; private set; }
    }
}