using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class AuthorizationService
    {
        public event Action UserAuthorized;
        public event Action<string> CaughtAuthorizationError;

        private FirebaseAuth _auth;
        private FirebaseUser _currentUser;

        public AuthorizationService() => InitializeFirebase();

        public async Task LoginWithEmail(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                CaughtAuthorizationError?.Invoke("Email или пароль не могут быть пустыми");
                return;
            }

            try
            {
                var authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                _currentUser = authResult.User;
                UserAuthorized?.Invoke();
                Debug.Log($"Успешная авторизация! ID пользователя: {_currentUser.UserId}");
            }
            catch (FirebaseException ex)
            {
                HandleFirebaseAuthError(ex);
            }
        }

        public async Task RegisterWithEmail(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                CaughtAuthorizationError.Invoke("Email или пароль не могут быть пустыми");
                return;
            }
            try
            {
                var authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                _currentUser = authResult.User;
                UserAuthorized?.Invoke();
                Debug.Log($"Регистрация успешна! ID пользователя: {_currentUser.UserId}");
            }
            catch (FirebaseException ex)
            {
                HandleFirebaseAuthError(ex);
            }
        }

        private void InitializeFirebase() => _auth = FirebaseAuth.DefaultInstance;


        private void HandleFirebaseAuthError(FirebaseException ex)
        {
            var errorCode = (AuthError)ex.ErrorCode;

            switch (errorCode)
            {
                case AuthError.InvalidEmail:
                    CaughtAuthorizationError?.Invoke("Некорректный формат почты.");
                    break;
                case AuthError.WrongPassword:
                    CaughtAuthorizationError?.Invoke("Неверный пароль.");
                    break;
                case AuthError.UserNotFound:
                    CaughtAuthorizationError?.Invoke("Пользователь не найден.");
                    break;
                case AuthError.UserDisabled:
                    CaughtAuthorizationError?.Invoke("Пользовательская учетная запись отключена.");
                    break;
                default:
                    CaughtAuthorizationError?.Invoke("Неизвестная ошибка: " + ex.Message);
                    break;
            }
        }
    }
}
