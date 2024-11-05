using Game.Data.Attributes;
using Game.Items;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Inventory.UI
{
    public class UIItemInventory : UIElement
    {
        [SerializeField, DataID(typeof(ItemsConfig))] private string _itemID;
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private Image _icon;
        
        private IInventoryOwner _inventoryOwner;
        private InventoryManager _inventoryManager;
        private ItemsManager _itemsManager;

        [Inject]
        private void Install(InventoryManager inventoryManager, ItemsManager itemsManager)
        {
            _inventoryManager = inventoryManager;
            _itemsManager = itemsManager;
        }

        private void Start()
        {
            _textField.text = _inventoryManager.GetItemCount(_itemID).ToString();
        }

        private void OnEnable()
        {
            if(_icon != null ) _icon.sprite = _itemsManager.GetData(_itemID).Icon;
            
            OnItemChanged(_inventoryOwner, _itemID, _inventoryManager.GetItemCount(_itemID));
            
            _inventoryManager.OnItemAdded += OnItemChanged;
            _inventoryManager.OnItemRemoved += OnItemChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _inventoryManager.OnItemAdded -= OnItemChanged;
            _inventoryManager.OnItemRemoved -= OnItemChanged;
        }

        private void OnItemChanged(IInventoryOwner owner, string item, int count)
        {
            if (!ReferenceEquals(owner, _inventoryOwner) || item != _itemID) return;

            _textField.text = _inventoryManager.GetItemCount(item, owner).ToString();
        }
    }
}