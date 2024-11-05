using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Characters
{
    public abstract class IHandlers<T> : SerializedMonoBehaviour where T : Component
    {
        [SerializeField] protected T _targetData;
    }
}