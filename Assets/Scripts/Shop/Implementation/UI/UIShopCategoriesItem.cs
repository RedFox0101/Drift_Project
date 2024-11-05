using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Shop.UI
{
    public class UIShopCategoriesItem : UIButton
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private UIShopCategoriesScreen _contract;

        private UIManager _uiManager;
        
        private ShopItemCategories _shopItemCategories;
        
        
        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void Initialize(ShopItemCategories shopItemCategories)
        {
            _shopItemCategories = shopItemCategories;
            _label.text = shopItemCategories.Name;
        }
        
        protected override void OnClick()
        {
            base.OnClick();
            
            var asset = _uiManager.ShowElement(_contract);
            
            asset.Initialize(_shopItemCategories);
        }
    }
}