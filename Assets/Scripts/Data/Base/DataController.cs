using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game.Data
{
    public abstract class DataController : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<IDataField> _dataFields;

        public event Action<IDataField> OnDataAdded;
        public event Action<IDataField> OnDataRemoved;
        
        public bool CanData<T>() where T : class, IDataField
        {
            return GetDataField<T>() != null;
        }
        
        public T[] GetDataFields<T>() where T : class, IDataField
        {
            return (T[])_dataFields.Where(field => field is T).ToArray();
        }
        
        public T GetDataField<T>(T dataField = null) where T : class, IDataField
        {
            Type targetType = ReferenceEquals(null, dataField) ? typeof(T) : dataField.GetType();
            return _dataFields.FirstOrDefault(field => field.GetType() == targetType) as T;
        }

        public bool TryGetDataField<T>(out T dataField) where T : class, IDataField
        {
            dataField = GetDataField<T>();
            
            return dataField != null;
        }

        public void AddDataField<T>(T dataField) where T : class, IDataField
        {
            _dataFields.Add(dataField);
            OnDataAdded?.Invoke(dataField);
        }
        
        public void RemoveDataField<T>(T dataField) where T : class, IDataField
        {
            _dataFields.Remove(dataField);
            OnDataRemoved?.Invoke(dataField);
        }
    }
}