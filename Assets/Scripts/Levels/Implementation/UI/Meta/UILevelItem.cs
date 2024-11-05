using System;
using Game.Predicates;
using Game.Server;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Levels.UI.Meta
{
    public class UILevelItem : UIElement
    {
        [SerializeField] private TMP_Text _labelField;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _lockMask;
        
        private LevelsManager _levelsManager;
        private ServerManager _serverManager;
        private PredicateManager _predicateManager;

        private string _levelDataID;
        
        [Inject]
        private void Install(LevelsManager levelsManager, PredicateManager predicateManager, ServerManager serverManager)
        {
            _levelsManager = levelsManager;
            _serverManager = serverManager;
            _predicateManager = predicateManager;
        }
        
        public void Initialize(string levelDataID)
        {
            _levelDataID = levelDataID;

            var levelData = _levelsManager.GetData(levelDataID);

            if (levelData != null)
            {
                _labelField.text = levelData.Name;

                bool isLock = _predicateManager.Check(levelData.Predicate);
                
                _lockMask.SetActive(!isLock);
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var levelData = _levelsManager.GetData(_levelDataID);

            if (levelData != null)
            {
                if (!_predicateManager.Check(levelData.Predicate)) return;

                Action setterLevelID = new Action(() =>
                {
                    var properties = _serverManager.GetRoomProperties();
                    
                    properties.Add(nameof(LevelDataScriptable), _levelDataID);
                    
                    _serverManager.SetRoomProperties(properties);
                });
                
                _serverManager.CreateRoom(levelData.LoadingDataID, setterLevelID);
            }
        }
    }
}