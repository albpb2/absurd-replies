using Mirror;
using TMPro;
using UnityEngine;

namespace AbsurdReplies
{
    public class GameStarter : NetworkBehaviour
    {
        [SerializeField]
        private NetworkManager _networkManager;
        [SerializeField]
        private TMP_InputField _portInputField;

        private ushort Port => ushort.Parse(_portInputField.text);
        
        private void Awake()
        {
            DependencyValidator.ValidateDependency(_portInputField, nameof(_portInputField), nameof(GameStarter));
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
            AbsurdRepliesNetworkManager.singleton.Transport.Port = Port;
        }
    }
}