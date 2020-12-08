using AbsurdReplies.Server;
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
            DependencyValidator.ValidateDependency(
                _lobbyPlayersObserver, 
                nameof(_lobbyPlayersObserver),
                nameof(StartGameButton));

            _button = GetComponent<Button>();
        }

        private async void Start()
        {
            if (!isServer)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _button.interactable = _lobbyPlayersObserver.PlayersCount >= GameSettings.Instance.MinPlayers;
        }
    }
}