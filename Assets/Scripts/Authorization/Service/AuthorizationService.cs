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
                CaughtAuthorizationError?.Invoke("Email ��� ������ �� ����� ���� �������");
                return;
            }

            try
            {
                var authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                _currentUser = authResult.User;
                UserAuthorized?.Invoke();
                Debug.Log($"�������� �����������! ID ������������: {_currentUser.UserId}");
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
                CaughtAuthorizationError.Invoke("Email ��� ������ �� ����� ���� �������");
                return;
            }
            try
            {
                var authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                _currentUser = authResult.User;
                UserAuthorized?.Invoke();
                Debug.Log($"����������� �������! ID ������������: {_currentUser.UserId}");
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
                    CaughtAuthorizationError?.Invoke("������������ ������ �����.");
                    break;
                case AuthError.WrongPassword:
                    CaughtAuthorizationError?.Invoke("�������� ������.");
                    break;
                case AuthError.UserNotFound:
                    CaughtAuthorizationError?.Invoke("������������ �� ������.");
                    break;
                case AuthError.UserDisabled:
                    CaughtAuthorizationError?.Invoke("���������������� ������� ������ ���������.");
                    break;
                default:
                    CaughtAuthorizationError?.Invoke("����������� ������: " + ex.Message);
                    break;
            }
        }
    }
}
