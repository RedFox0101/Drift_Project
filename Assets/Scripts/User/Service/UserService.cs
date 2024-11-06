using Firebase.Auth;
using Game.Loadings;

namespace Game
{
    public class UserService
    {
        private UserData _currentUser;
        private FirebaseDatabaseService _firebaseDatabaseService;
        private LoadingService _loadingManager;

        public UserData CurrentUser => _currentUser;

        public UserService(FirebaseDatabaseService firebaseDatabaseService, LoadingService loadingManager)
        {
            _firebaseDatabaseService = firebaseDatabaseService;
            _loadingManager = loadingManager;
        }

        public async void Setup()
        {
            _currentUser = await _firebaseDatabaseService.GetUserDataAsync();
            _loadingManager.LoadMeta();
        }

        public async void CreateUser(string userName)
        {
            _currentUser = new UserData()
            {
                Name = userName
            };

            await _firebaseDatabaseService.SaveUserDataAsync(_currentUser);
            Setup();

        }

        public void SetNickName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                return;

            _currentUser.Name = newName;
            _firebaseDatabaseService.SaveUserDataAsync(_currentUser);
        }
    }
}
