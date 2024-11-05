using Game.Assets;
using UnityEngine;
using Zenject;

namespace Game.UI.UIElements
{
    public class UIButtonScreenActive : UIButton
    {
        [SerializeField] protected UIScreen _uiScreen;
        [SerializeField] protected bool _isActiveScreen;

        protected UIManager _uiManager;
        protected UIElement _uiElementSpawn;
        
        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        private void OnDestroy()
        {
            if (_uiElementSpawn != null) OnReleaseUIElement(_uiElementSpawn);
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (_isActiveScreen)
            {
                if (_uiElementSpawn == null)
                {
                    _uiElementSpawn = _uiManager.ShowElement(_uiScreen);
                    _uiElementSpawn.OnReleased += OnReleaseUIElement;
                }
            }
            else
            {
                _uiManager.HideElement(_uiScreen);
            }
        }

        private void OnReleaseUIElement(IAsset asset)
        {
            _uiElementSpawn.OnReleased -= OnReleaseUIElement;
            _uiElementSpawn = null;
        }
    }
}