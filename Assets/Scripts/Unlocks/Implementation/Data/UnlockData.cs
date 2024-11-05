using Game.Data.Attributes;
using Game.Data.DataFields.Base;
using Game.Predicates;
using Game.Rewards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Unlocks.Data
{
    public class UnlockData : StringData, IRewardData, IPredicateData
    {
        [DataID(typeof(UnlockConfig))]
        [SerializeField, OnValueChanged("OnValueChanged")] protected new string _value;
        
        public override string Value => _value;
    }
}