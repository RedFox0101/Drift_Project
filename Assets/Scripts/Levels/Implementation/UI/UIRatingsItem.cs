using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Levels.UI
{
    public class UIRatingsItem : UIElement
    {
        [SerializeField] private TMP_Text _playerTextField;
        [SerializeField] private TMP_Text _scoreTextField;
        [SerializeField] private Image _background;
        [SerializeField] private Color _colorIsMine;

        private Color _defaultColor;
        
        private void Awake()
        {
            _defaultColor = _background.color;
        }

        public void SetPlayerName(string value)
        {
            _playerTextField.text = value;
        }
        
        public void SetScore(string value)
        {
            _scoreTextField.text = value;
        }

        public void SetIsMine(bool isMine)
        {
            _background.color = isMine ? _colorIsMine : _defaultColor;
        }
    }
}