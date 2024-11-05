using Game.UI;
using Zenject;

namespace Game.Server.UI
{
    public class UISingleGameScreen : UIScreen
    {
        private ServerManager _serverManager;

        [Inject]
        private void Install(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        private void OnEnable()
        {
            _serverManager.SetSingleGame(true);
        }
    }
}