using System.Collections.Generic;
using Game.UI;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Shop.UI
{
    public class UIShopScreen : UIScreen
    {
        [SerializeField] private Transform _targetContainer;
        [SerializeField] private UIShopCategoriesItem contractShopCategoriesItem;
        [SerializeField] private ShopItemCategories[] _shopItemCategories;
        
        private UIManager _uiManager;
        private List<UIShopCategoriesItem> _shopCategoriesPool = new();
        
        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        private void OnEnable()
        {
            _shopItemCategories.ForEach(item =>
            {
                var uiElement = _uiManager.ShowElement(contractShopCategoriesItem, _targetContainer);
                uiElement.Initialize(item);
                _shopCategoriesPool.Add(uiElement);
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _shopCategoriesPool.ForEach(item => _uiManager.HideElement(item));
            _shopCategoriesPool.Clear();
        }
    }
}