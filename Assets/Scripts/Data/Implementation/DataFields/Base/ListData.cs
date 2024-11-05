using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data.DataFields.Base
{
    public class ListData<T> : IDataField<List<T>>
    {
        public event Action<T> OnItemAdded;
        
        public event Action<T> OnItemRemoved;
        
        public void Add(T item)
        {
            _value ??= new();

            _value.Add(item);
            
            OnItemAdded?.Invoke(item);
            OnValueChanged(_value);
        }

        public void Remove(T item)
        {
            if (_value == null) return;

            _value.Remove(item);
            
            OnItemRemoved?.Invoke(item);
            OnValueChanged(_value);
        }

        public void Clear()
        {
            _value?.ToList().ForEach(Remove);
        }
    }
}