using System.Collections.Generic;
using Game.UI;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Equipments.UI
{
    public class UICustomizationScreen : UIScreen
    {
        [SerializeField] private UICustomizationSlotButton _customizationSlotContract;
        [SerializeField] private Transform _container;
        
        private UIManager _uiManager;
        private EquipmentManager _equipmentManager;

        private List<UICustomizationSlotButton> _slotPools = new();
        
        [Inject]
        private void Install(UIManager uiManager, EquipmentManager equipmentManager)
        {
            _uiManager = uiManager;
            _equipmentManager = equipmentManager;
        }

        private void OnEnable()
        {
            _equipmentManager.GetDataAll().ForEach(data =>
            {
                var asset = _uiManager.ShowElement(_customizationSlotContract, _container);

                asset.Initialize(data);

                _slotPools.Add(asset);
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _slotPools.ForEach(slot => _uiManager.HideElement(slot));
            _slotPools.Clear();
        }
        
    }
}