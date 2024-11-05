using System;
using System.Collections.Generic;
using Game.Data;
using Game.Save;
using Sirenix.Utilities;
using Zenject;

namespace Game.Settings
{
    public class SettingsManager : DataManager<SettingsDataScriptable, SettingsConfig>
    {
        private const string _saveID = nameof(SettingsManager);
        
        private Dictionary<string, bool> _settingsValues = new();

        public event Action<string, bool> OnSettingsChanged;

        private SaveManager _saveManager;

        [Inject]
        private void Install(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }
        
        protected override void Initialized()
        {
            base.Initialized();

            if (_saveManager.TryGetData<Dictionary<string, bool>>(_saveID, out var result))
                _settingsValues = result;
            
            _config.Datas.ForEach(data => _settingsValues.TryAdd(data.ID, data.DefaultValue));
            
            _config.BaseSettings?.ForEach(baseSetting => baseSetting.Initialize());
        }

        public void SetSettingValue(string settingsID, bool value)
        {
            if (_settingsValues.ContainsKey(settingsID) && !_settingsValues[settingsID].Equals(value))
            {
                _settingsValues[settingsID] = value;

                _saveManager.SetData(_saveID, _settingsValues);
                
                OnSettingsChanged?.Invoke(settingsID, value);
            }
        }

        public bool GetSettingsValue(string settingsID)
        {
            base.Initialize();
            
            return _settingsValues[settingsID];
        }
    }
}