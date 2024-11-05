using Game.Data;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Data/Settings/Settings Data", fileName = "Settings Data")]
    public class SettingsDataScriptable : DataScriptable
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public bool DefaultValue { get; private set; }
    }
}