using Game.Data;
using UnityEngine;

namespace Game.Unlocks
{
    [CreateAssetMenu(menuName = "Data/Unlock/Unlock Data", fileName = "Unlock Data")]
    public class UnlockDataScriptable : DataScriptable
    {
        [field:SerializeField] public string Desc { get; private set; } 
    }
}