using Game.Data;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Data/Settings/Settings Config", fileName = "Settings Config")]
    public class SettingsConfig : DataConfig<SettingsDataScriptable>
    {
        [SerializeField] public IBaseSettings[] BaseSettings { get; private set; }
    }
}