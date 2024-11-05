using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Data
{
    public abstract class DataScriptable : SerializedScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
    }
}