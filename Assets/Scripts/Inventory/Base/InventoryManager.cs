using System;
using System.Collections.Generic;
using Game.Characters.Character;
using Game.Items;
using Game.Save;
using Sirenix.Utilities;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Inventory
{
    public class InventoryManager : IInitializable
    {
        private const string _saveID = nameof(InventoryManager);
        
        private SaveManager _saveManager;
        private ItemsManager _itemsManager;
        private Dictionary<string, Dictionary<string, int>> _items = new();

        public event Action<IInventoryOwner, string, int> OnItemChanged;
        public event Action<IInventoryOwner, string, int> OnItemAdded;
        public event Action<IInventoryOwner, string, int> OnItemRemoved;
        
        [Inject]
        private void Install(ItemsManager itemsManager, SaveManager saveManager)
        {
            _saveManager = saveManager;
            _itemsManager = itemsManager;
        }

        public void Initialize()
        {
            if (_saveManager.TryGetData<Dictionary<string, Dictionary<string, int>>>(_saveID, out var result))
                _items = result;
        }

        public bool CanAddItem(string targetItem, int count = 1, IInventoryOwner owner = null)
        {
            var itemData = _itemsManager.GetData(targetItem);

            if (itemData != null)
            {
                string ownerID = GetOwnerID(owner);
                
                int currentCount = GetItemCount(targetItem, owner);

                return itemData.StackLimit == -1 || currentCount < itemData.StackLimit;
            }

            return false;
        }
        
        public void Add(string targetItem, int count, IInventoryOwner owner = null)
        {
            var itemData = _itemsManager.GetData(targetItem);

            if (itemData != null)
            {
                string ownerID = GetOwnerID(owner);
                
                owner = GetOwner(owner);
                
                if (!_items.ContainsKey(ownerID)) _items.Add(ownerID, new Dictionary<string, int>());

                if (!_items[ownerID].ContainsKey(targetItem)) _items[ownerID].Add(targetItem, 0);

                var resultCount = _items[ownerID][targetItem] + count;

                if (itemData.StackLimit != -1) resultCount = Math.Clamp(resultCount, resultCount, itemData.StackLimit);

                var oldCount = _items[ownerID][targetItem]; 
                
                _items[ownerID][targetItem] = resultCount;

                _saveManager.SetData(_saveID, _items);
                
                OnItemChanged?.Invoke(owner, targetItem, resultCount);
                OnItemAdded?.Invoke(owner, targetItem, resultCount - oldCount);
            }
        }

        public void Remove(string targetItem, int count, IInventoryOwner owner = null)
        {
            string ownerID = GetOwnerID(owner);
            owner = GetOwner(owner);
            
            if (_items.TryGetValue(ownerID, out var items))
            {
                if (items.ContainsKey(targetItem))
                {
                    var resultCount = items[targetItem] - count;
                    resultCount = Math.Clamp(resultCount, 0, resultCount);
                    
                    var oldCount = _items[ownerID][targetItem];
                    
                    items[targetItem] = resultCount;

                    _saveManager.SetData(_saveID, _items);
                    
                    OnItemChanged?.Invoke(owner, targetItem, resultCount);
                    OnItemRemoved?.Invoke(owner, targetItem, resultCount - oldCount);
                }
            }
        }

        public bool ContainsItem(string targetItem, int count = 1, IInventoryOwner owner = null)
        {
            return GetItemCount(targetItem, owner) >= count;
        }

        public int GetItemCount(string targetItem, IInventoryOwner owner = null)
        {
            string ownerID = GetOwnerID(owner);
            if (_items.TryGetValue(ownerID, out var items))
            {
                if (items.TryGetValue(targetItem, out int itemCount))
                {
                    return itemCount;
                }
            }
            return 0;
        }

        public int GetItemAllCount(IInventoryOwner owner)
        {
            var items = GetItems(owner);

            int count = 0;

            items?.ForEach(item => count += item.Value);

            return count;
        }

        public Dictionary<string, int> GetItems(IInventoryOwner owner)
        {
            string ownerID = GetOwnerID(owner);
            return _items.TryGetValue(ownerID, out var inventory) ? inventory : null;
        }

        private IInventoryOwner GetOwner(IInventoryOwner owner)
        {
            return owner ?? Object.FindObjectOfType<PlayerController>();
        }

        private string GetOwnerID(IInventoryOwner owner)
        {
            return ReferenceEquals(owner, null) ? PlayerController.PlayerID : owner.InventoryOwnerID;
        }
    }
}