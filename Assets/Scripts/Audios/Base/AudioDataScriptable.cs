using Game.Data;
using Game.Data.Attributes;
using Game.Settings;
using UnityEngine;

namespace Game.Audios
{
    [CreateAssetMenu(menuName = "Data/Audio/Audio Data", fileName = "Audio Data")]
    public class AudioDataScriptable : DataScriptable
    {
        [field: SerializeField] public AudioType AudioType { get; private set; }
        [field: SerializeField, DataID(typeof(SettingsConfig))] public string SettingsID { get; private set; }
    }
}