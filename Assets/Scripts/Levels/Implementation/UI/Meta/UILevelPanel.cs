using System.Collections.Generic;
using Game.UI;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Levels.UI.Meta
{
    public class UILevelPanel : UIElement
    {
        [SerializeField] private UILevelItem _uiLevelItemContract;
        [SerializeField] private Transform _container;
        
        private UIManager _uiManager;
        private LevelsManager _levelsManager;

        private List<UILevelItem> _levelItemsPool = new();
        
        
        [Inject]
        private void Install(UIManager uiManager, LevelsManager levelsManager)
        {
            _uiManager = uiManager;
            _levelsManager = levelsManager;
        }

        private void OnEnable()
        {
            var levelIDs = _levelsManager.GetIDAll();

            levelIDs?.ForEach(levelID =>
            {
                var uiLevelItem = _uiManager.ShowElement(_uiLevelItemContract, _container);
                _levelItemsPool.Add(uiLevelItem);
                
                uiLevelItem.Initialize(levelID);
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _levelItemsPool.ForEach(levelItem => _uiManager.HideElement(levelItem));
            
            _levelItemsPool.Clear();
        }
    }
}