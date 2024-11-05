using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Equipments.UI
{
    public class UICustomizationSlotButton : UIButton
    {
        [SerializeField] private UICustomizationSlotScreen _slotScreenContract;
        [SerializeField] private TMP_Text _labelTextField;
        
        private UIManager _uiManager;
        
        private EquipmentDataScriptable _equipmentData;
        
        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public void Initialize(EquipmentDataScriptable equipmentData)
        {
            _equipmentData = equipmentData;
            
            if (_equipmentData != null)
            {
                _labelTextField.text = equipmentData.Name;   
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            
            var asset = _uiManager.ShowElement(_slotScreenContract);
            
            asset.Initialize(_equipmentData);
        }
    }
}