using System.Threading.Tasks;
using AbsurdReplies.Exceptions;
using Mirror;
using UnityEngine;

namespace AbsurdReplies
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField]
        private NetworkManager _networkManager;
        [SerializeField]
        private TMPro.TMP_InputField _gameIdSource;

        public string GameId => _gameIdSource.text;
        
        private void Awake()
        {
            DependencyValidator.ValidateDependency(_gameIdSource, nameof(_gameIdSource), nameof(GameStarter));
            DependencyValidator.ValidateDependency(_networkManager, nameof(_networkManager), nameof(NetworkManager));
        }

        public async void StartHost()
        {
            await ValidateGameId();
            _networkManager.StartHost();
        }

        public async void JoinLocal()
        {
            await ValidateGameId();
            _networkManager.StartClient();
        }

        private async Task ValidateGameId()
        {
            if (string.IsNullOrWhiteSpace(GameId))
            {
                throw ExceptionBecause.GameIdMissing();
            }
        }
    }
}