using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Data
{
    public interface IDataField
    {
        void SetInstance(IDataField dataField);
    }
    
    [Serializable]
    public class IDataField<T> : IDataField
    {
        [SerializeField, OnValueChanged("OnValueChanged")] protected T _value;

        public virtual T Value => _value;

        public event Action<T> OnChanged;
        public event Action<IDataField<T>> OnDataChanged;

        public virtual void SetValue(T value)
        {
            SetValue(ref _value, value);
        }
        
        protected void SetValue(ref T refValue, T newValue)
        {
            if (!Equals(refValue, newValue))
            {
                refValue = newValue;
                OnChanged?.Invoke(newValue);
                OnDataChanged?.Invoke(this);
            }
        }

        protected virtual void OnValueChanged(T value)
        {
            OnChanged?.Invoke(value);
            OnDataChanged?.Invoke(this);
        }

        protected virtual bool Equals(T oldValue, T newValue)
        {
            return oldValue == null && newValue == null || oldValue != null && oldValue.Equals(newValue);
        }
        
        public virtual void SetInstance(IDataField dataField)
        {
            if (dataField is IDataField<T> data)
            {
                if (_value == null && data.Value != null || !_value.Equals(data.Value))
                {
                    SetValue(data.Value);
                }
            }
        }
    }
}