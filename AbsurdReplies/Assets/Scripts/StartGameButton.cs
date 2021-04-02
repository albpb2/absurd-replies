using AbsurdReplies.Server;
using AbsurdReplies.Dependencies;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace AbsurdReplies
{
    public class StartGameButton : NetworkBehaviour
    {
        [SerializeField] private LobbyPlayersObserver _lobbyPlayersObserver;

        private Button _button;

        private async void Awake()
        {
            DependencyValidator.ValidateDependency(_lobbyPlayersObserver, nameof(_lobbyPlayersObserver));

            _button = GetComponent<Button>();
        }

        private async void Start()
        {
            if (!isServer)
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        { 
            NetworkManager.singleton.ServerChangeScene(SceneNames.GameScene);
        }

        private void Update()
        {
            _button.interactable = _lobbyPlayersObserver.PlayersCount >= GameSettings.Instance.MinPlayers;
        }
    }
}