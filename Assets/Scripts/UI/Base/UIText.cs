using TMPro;
using UnityEngine;

namespace Game.UI.Base
{
    public class UIText : UIElement
    {
        [SerializeField] private TMP_Text _textField;

        public void SetText(string value)
        {
            _textField.text = value;
        }
    }
}