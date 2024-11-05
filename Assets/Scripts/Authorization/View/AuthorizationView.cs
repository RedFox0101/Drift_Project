using Firebase.Auth;
using Game.Loadings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public class AuthorizationView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private ErrorMessageView _errorMessageView;

        private AuthorizationService _authorizationService;
        private UserService _userService;

        [Inject]
        public void Istall(AuthorizationService authorizationService, UserService userService)
        {
            _authorizationService= authorizationService;
            _authorizationService.CaughtAuthorizationError += OnCaughtAuthorizationError;
            _authorizationService.UserAuthorized += OnUserAuthorized;

            _userService = userService;
        }

        public void StartAuthorization() => _authorizationService.LoginWithEmail(_emailInputField.text, _passwordInputField.text);

        private void Start() => TrySilentSignIn();

        private void TrySilentSignIn()
        {
            if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            {
                
            }
        }

        private void OnCaughtAuthorizationError(string error) => _errorMessageView.ShowError(error);

        private void OnUserAuthorized()
        {
            _userService.Setup();
        }

        private void OnDestroy()
        {
            _authorizationService.CaughtAuthorizationError -= OnCaughtAuthorizationError;
        }
    }
}
