using System.Collections.Generic;
using Game.Data.Attributes;
using Game.UI;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Settings.UI
{
    public class UISettingsScreen : UIScreen
    {
        [SerializeField] private UISettingsItem _uiSettingsItemContract;
        [SerializeField, DataID(typeof(SettingsConfig))] private string[] _settingsIDs;
        [SerializeField] private Transform _container;
        
        private UIManager _uiManager;

        private List<UISettingsItem> _settingsItemsPool = new();

        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        private void OnEnable()
        {
            _settingsIDs?.ForEach(settingID =>
            {
                var uiSettingsItem = _uiManager.ShowElement(_uiSettingsItemContract, _container);

                _settingsItemsPool.Add(uiSettingsItem);

                uiSettingsItem.Initialize(settingID);
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _settingsItemsPool.ForEach(settingsItem => _uiManager.HideElement(settingsItem));
            _settingsItemsPool.Clear();
        }
    }
}