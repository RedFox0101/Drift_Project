using TMPro;
using UnityEngine;

namespace Game
{
    public class ErrorMessageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private GameObject _errorContainer;

        public void ShowError(string errorMessage)
        {
            _errorContainer.SetActive(true);
            _errorText.text = errorMessage;
        }
    }
}
