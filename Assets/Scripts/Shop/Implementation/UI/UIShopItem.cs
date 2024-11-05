using Game.Inventory;
using Game.Items;
using Game.Items.Data;
using Game.Rewards;
using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Shop.UI
{
    public class UIShopItem : UIButton
    {
        [SerializeField] private TMP_Text _rewardTextField;
        [SerializeField] private TMP_Text _priceTextField;

        private UIManager _uiManager;
        private ShopManager _shopManager;
        private ItemsManager _itemsManager;
        private RewardsManager _rewardsManager;
        private InventoryManager _inventoryManager;

        private ShopItemInfo _shopItemInfo;

        private IPriceData _price;
        private IRewardData _rewardData;
        
        [Inject]
        private void Install(UIManager uiManager, ShopManager shopManager, ItemsManager itemsManager, 
            RewardsManager rewardsManager, InventoryManager inventoryManager)
        {
            _uiManager = uiManager;
            _shopManager = shopManager;
            _itemsManager = itemsManager;
            _rewardsManager = rewardsManager;
            _inventoryManager = inventoryManager;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _inventoryManager.OnItemAdded += OnItemAdd;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inventoryManager.OnItemAdded -= OnItemAdd;
        }

        public void Initialize(ShopItemInfo shopItemInfo)
        {
            _shopItemInfo = shopItemInfo;
            SetPrice(shopItemInfo.Price);
            SetReward(shopItemInfo.Reward);
        }

        private void OnItemAdd(IInventoryOwner arg1, string itemID, int count)
        {
            if (_shopItemInfo.Reward is ItemData itemReward && _inventoryManager.CanAddItem(itemReward.Value, itemReward.Count))
            {
                if (itemReward.Value.Equals(itemID))
                {
                    _uiManager.HideElement(this);
                }
            }
        }

        private void SetPrice(IPriceData price)
        {
            _price = price;
            
            if (price is ItemData itemData)
            {
                var item = _itemsManager.GetData(itemData.Value);
                _priceTextField.text = $"{item.Name}: {itemData.Count.ToString()}";
            }
            else
            {
                _priceTextField.text = "No sale";
            }
        }
        
        private void SetReward(IRewardData rewardData)
        {
            _rewardData = rewardData;
            
            if (rewardData is ItemData itemData)
            {
                var item = _itemsManager.GetData(itemData.Value);

                if (item != null)
                {
                    _rewardTextField.text = item.Name;
                }
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            
            if (_shopManager.TryBuy(_price))
            {
                _rewardsManager.Reward(_rewardData);
            }
        }
    }
}