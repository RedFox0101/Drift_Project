using Game.Server;
using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Settings.UI
{
    public class UISettingsNickName : UIElement
    {
        [SerializeField] private TMP_InputField _inputField;
        
        private ServerManager _serverManager;

        [Inject]
        private void Install(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        private void OnEnable()
        {
            _inputField.text = _serverManager.GetNickName();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _serverManager.SetNickName(_inputField.text);
        }
    }
}