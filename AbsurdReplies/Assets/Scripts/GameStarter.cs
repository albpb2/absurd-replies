using Mirror;
using UnityEngine;

namespace AbsurdReplies
{
    public class GameStarter : NetworkBehaviour
    {
        [SerializeField]
        private NetworkManager _networkManager;
        [SerializeField]
        private PortInputText _portSource;
        
        private void Awake()
        {
            DependencyValidator.ValidateDependency(_portSource, nameof(_portSource), nameof(GameStarter));
            DependencyValidator.ValidateDependency(_networkManager, nameof(_networkManager), nameof(NetworkManager));
        }

        public async void StartHost()
        {
            UpdatePort();
            _networkManager.StartHost();
        }

        public async void JoinLocal()
        {
            UpdatePort();
            _networkManager.StartClient();
        }

        private void UpdatePort()
        {
            AbsurdRepliesNetworkManager.singleton.Transport.Port = _portSource.Port;
        }
    }
}