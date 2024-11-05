using System.Collections.Generic;
using Game.Characters.Character;
using Game.Characters.Datas;
using Game.Items;
using Game.Items.Data;
using Game.Loadings;
using Game.Server;
using Game.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Levels.UI
{
    public class UILevelWinScreen : UIScreen
    {
        [SerializeField] private UIRatingsItem _uiRatingsItemContract;
        [SerializeField] private Transform _ratingsContainer;
        
        [SerializeField] private TMP_Text _textRewardField;
        [SerializeField] private UIButton _buttonOK;

        private UIManager _uiManager;
        private ItemsManager _itemsManager;
        private ServerManager _serverManager;
        private LoadingManager _loadingManager;

        private List<UIRatingsItem> _uiRatingsItemPool;
        
        [Inject]
        private void Install(UIManager uiManager, ItemsManager itemsManager, ServerManager serverManager, LoadingManager loadingManager)
        {
            _uiManager = uiManager;
            _itemsManager = itemsManager;
            _serverManager = serverManager;
            _loadingManager = loadingManager;
        }
        
        public void Initialize(ItemData itemData)
        {
            ItemDataScriptable data = _itemsManager.GetData(itemData.Value);

            if (data != null)
            {
                _textRewardField.text = $"{data.Name}: {itemData.Count}";
            }
        }

        private void OnEnable()
        {
            _buttonOK.OnTap += OnButtonClick;

            List<KeyValuePair<PlayerController, int>> playersScores = new();
            _serverManager.ServerResourcesPool.ForEach(serverResource =>
            {
                var player = serverResource.GetComponentInChildren<PlayerController>();
                if (player != null)
                {
                    var scorePoints = player.GetDataField<ScorePoints>();
                    if (scorePoints != null)
                    {
                        playersScores.Add(new(player, scorePoints.Value));
                    }
                }
            });

            playersScores.Sort((x, y) => y.Value.CompareTo(x.Value));

            playersScores.ForEach(playerScore =>
            {
                var uiRatingsItem = _uiManager.ShowElement(_uiRatingsItemContract, _ratingsContainer);
                uiRatingsItem.SetPlayerName(playerScore.Key.PhotonView.Controller.NickName);
                uiRatingsItem.SetScore(playerScore.Value.ToString());
                uiRatingsItem.SetIsMine(playerScore.Key.PhotonView.IsMine);
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _buttonOK.OnTap -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            _serverManager.Disconnect();
            _loadingManager.LoadMeta();
        }
    }
}