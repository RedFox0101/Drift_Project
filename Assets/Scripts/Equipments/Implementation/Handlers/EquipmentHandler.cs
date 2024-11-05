using System.Collections.Generic;
using System.Linq;
using Game.Characters;
using Game.Characters.Character;
using Game.Data.Attributes;
using Game.Equipments.Datas;
using Game.Equipments.ItemData;
using Game.Items;
using UnityEngine;
using Zenject;

namespace Game.Equipments.Implementation.Handlers
{
    public abstract class EquipmentHandler<T, TObject> : IHandlers<PlayerController> where T : Component where TObject : Object
    {
        [SerializeField] protected T _targetSlot;
        [SerializeField, DataID(typeof(EquipmentConfig))] protected string _equipmentID;

        protected ItemsManager _itemsManager;
        protected EquipmentsData _equipmentsData;

        protected TObject _defaultItem;

        [Inject]
        private void Install(ItemsManager itemsManager)
        {
            _itemsManager = itemsManager;
        }

        private void Awake()
        {
            _equipmentsData = _targetData.GetDataField<EquipmentsData>();
            _defaultItem = GetDefaultItem();
        }

        protected abstract TObject GetDefaultItem();

        protected virtual void OnEnable()
        {
            _equipmentsData.OnChanged += OnEquipmentChanged;
            OnEquipmentChanged(_equipmentsData.Value);
        }

        protected virtual void OnDisable()
        {
            _equipmentsData.OnChanged -= OnEquipmentChanged;
            ResetEquipment();
        }
        
        private void OnEquipmentChanged(List<string> equipmentsList)
        {
            ResetEquipment();
            var equipItemData = equipmentsList?.Select(itemID => _itemsManager.GetData(itemID) as EquipItemDataScriptable).
                FirstOrDefault(itemData => itemData != null && itemData.EquipmentSlotID.Equals(_equipmentID));

            if (equipItemData != null && equipItemData.ObjectVisual is TObject objectVisual)
            {
                EquipItem(equipItemData, objectVisual);
            }
        }
        
        protected abstract void EquipItem(EquipItemDataScriptable equipItemData, TObject objectVisual);

        protected abstract void ResetEquipment();
    }
}