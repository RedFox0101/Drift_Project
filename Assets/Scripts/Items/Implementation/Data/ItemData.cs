using System;
using Game.Data;
using Game.Data.Attributes;
using Game.Data.DataFields.Base;
using Game.Predicates;
using Game.Rewards;
using Game.Shop;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Items.Data
{
    public class ItemData : StringData, IRewardData, IPriceData, IPredicateData
    {
        [DataID(typeof(ItemsConfig))]
        [SerializeField, OnValueChanged("OnValueChanged")] protected new string _value;
        [SerializeField] private int _count = 1;
        
        public override string Value => _value;
        
        public int Count => _count;
        
        public event Action<int> OnCountValueChanged;

        public void SetCount(int count)
        {
            if (!_count.Equals(count))
            {
                _count = count;
                
                OnCountValueChanged?.Invoke(count);
            }
        }

        public override void SetValue(string value)
        {
            SetValue(ref _value, value);
        }

        public override void SetInstance(IDataField dataField)
        {
            base.SetInstance(dataField);
            
            if (dataField is ItemData data)
            {
                if (!_count.Equals(data.Count))
                {
                    SetCount(data.Count);
                }
            }
        }
    }
}