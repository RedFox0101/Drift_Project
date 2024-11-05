using UnityEngine;
using Zenject;

namespace Game.UI.UIElements
{
    //TODO: need rename
    public class UIElementActiveChanged : UIButton
    {
        [SerializeField] private UIElement _uiElement;
        [SerializeField] private bool _isActive;
        
        private UIManager _uiManager;
        
        [Inject]
        private void Construct(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        protected override void OnClick()
        {
            if (_isActive) _uiManager.ShowElement(_uiElement);
            else _uiManager.HideElement(_uiElement);
        }
    }
}