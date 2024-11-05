using Game.UI.UIElements;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Settings.UI
{
    public class UISettingsItem : UIToggle
    {
        [SerializeField] private TMP_Text _textField;
        
        private SettingsManager _settingsManager;

        private string _settingsID;
        
        [Inject]
        private void Install(SettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        
        public void Initialize(string settingsID)
        {
            _settingsID = settingsID;

            var settingsData = _settingsManager.GetData(settingsID);

            if (settingsData != null)
            {
                _textField.text = settingsData.Name;
                _toggle.isOn = _settingsManager.GetSettingsValue(settingsID);
            }
        }

        protected override void OnToggleChanged(bool value)
        {
            base.OnToggleChanged(value);
            _settingsManager.SetSettingValue(_settingsID, value);
        }
    }
}