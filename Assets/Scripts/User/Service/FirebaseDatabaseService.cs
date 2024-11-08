using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class FirebaseDatabaseService
    {
        private const string PathString = "Users";
        private DatabaseReference _databaseReference;

        public FirebaseDatabaseService()
        {
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async Task SaveUserDataAsync(UserData userData)
        {
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            if (userData == null)
            {
                Debug.LogError("UserData �� ����� ���� ������");
                return;
            }
            try
            {
                string jsonData = JsonUtility.ToJson(userData);
                await _databaseReference.Child(PathString).Child(userId).SetRawJsonValueAsync(jsonData);
            }
            catch (FirebaseException ex)
            {
                Debug.LogError($"������ ��� ���������� ������: {ex.Message}");
            }
        }

        public async Task<UserData> GetUserDataAsync()
        {
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("userId �� ����� ���� ������");
                return null;
            }

            try
            {
                DataSnapshot snapshot = await _databaseReference.Child(PathString).Child(userId).GetValueAsync();
                if (snapshot.Exists)
                {
                    string jsonData = snapshot.GetRawJsonValue();
                    UserData userData = JsonUtility.FromJson<UserData>(jsonData);
                  
                    return userData;
                }
                else
                {
                    FirebaseAuth.DefaultInstance.SignOut();
                    Debug.LogWarning("������ ������������ �� �������");
                    return null;
                }
            }
            catch (FirebaseException ex)
            {
                Debug.LogError($"������ ��� ��������� ������: {ex.Message}");
                return null;
            }
        }
    }
}
