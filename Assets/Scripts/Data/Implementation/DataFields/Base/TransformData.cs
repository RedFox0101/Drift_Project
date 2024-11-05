using Game.Data;
using UnityEngine;

namespace Data.Implementation.DataFields.Base
{
    // [Serializable]
    public abstract class TransformData : IDataField<Transform>
    {
        protected override bool Equals(Transform oldValue, Transform newValue)
        {
            return oldValue == newValue;
        }
    }
}