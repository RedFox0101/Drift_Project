using Game.Assets;
using Game.Characters.Character;
using Game.Characters.Datas;
using Game.Characters.UI;
using Game.UI;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Characters.Handlers.Player
{
    public class PlayerDriftPoint : IHandlers<PlayerController>
    {
        [SerializeField] private float _thresholdPoints = 0.75f;
        [SerializeField] private float _factorPoints = 1f;
        [SerializeField] private UIDriftPointPanel _driftPointPanelContract;
        
        private float _currentPoints;
        
        private float _lastPoints;

        private ScorePoints _scorePoints;

        private UIManager _uiManager;

        private UIDriftPointPanel _driftPointPanel;

        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        private void Awake()
        {
            _scorePoints = _targetData.GetDataField<ScorePoints>();
        }

        private void OnEnable()
        {
            _targetData.WheelController.ForEach(wheel => wheel.WheelSkid.OnSkidTotal += OnWheelSkid);
        }

        private void OnDisable()
        {
            _targetData.WheelController.ForEach(wheel => wheel.WheelSkid.OnSkidTotal -= OnWheelSkid);
            
            if(_driftPointPanel != null) _uiManager.HideElement(_driftPointPanel);
        }

        private void OnWheelSkid(float value)
        {
            if (value < _thresholdPoints) return;

            _currentPoints += value * _factorPoints;
        }

        private void Update()
        {
            if (_currentPoints == _lastPoints)
            {
                _scorePoints.IncreaseValue((int)_currentPoints);
                _currentPoints = 0f;
            }
            else
            {
                if (_driftPointPanel == null)
                {
                    _driftPointPanel = _uiManager.ShowElement(_driftPointPanelContract);
                    _driftPointPanel.OnReleased += OnDriftPanelRelease;
                }
                
                _driftPointPanel.SetPoints((int)_currentPoints);
                
            }
            _lastPoints = _currentPoints;
        }

        private void OnDriftPanelRelease(IAsset asset)
        {
            if (_driftPointPanel == null) return;
            
            _driftPointPanel.OnReleased -= OnDriftPanelRelease;
            _driftPointPanel = null;
        }
    }
}