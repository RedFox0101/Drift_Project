using Game.Loadings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public class RegistrationView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _userNameInputField;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _repeatPasswordInputField;
        [SerializeField] private ErrorMessageView _errorMessageView;

        private AuthorizationService _authorizationService;
        private UserService _userService;

        [Inject]
        public void Istall(AuthorizationService authorizationService, UserService userService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
            _authorizationService.CaughtAuthorizationError += OnCaughtAuthorizationError;
            _authorizationService.UserAuthorized += OnUserAuthorized;
        }

        public void StartRegistration()
        {
            if (_passwordInputField.text != _repeatPasswordInputField.text)
            {
                return;
            }

            _authorizationService.RegisterWithEmail(_emailInputField.text, _passwordInputField.text);
        }

        private void OnUserAuthorized() => _userService.CreateUser(_userNameInputField.text);

        private void OnCaughtAuthorizationError(string errorMessage) => _errorMessageView.ShowError(errorMessage);

        private void OnDestroy()
        {
            _authorizationService.CaughtAuthorizationError -= OnCaughtAuthorizationError;
        }

    }
}
