using System.Collections.Generic;
using System.Linq;
using AbsurdReplies.Player;
using Mirror;
using UnityEngine;

namespace AbsurdReplies.Server
{
    public class LobbyPlayersObserver : NetworkBehaviour
    {
        [SerializeField] private LobbyPlayersListUpdater _lobbyPlayersListUpdater;
        
        private Dictionary<int, AbsurdRepliesPlayer> _players;
        
        private void Awake()
        {
            if (isClientOnly)
            {
                Debug.Log($"Not server, destroying {nameof(LobbyPlayersObserver)}");
                Destroy(gameObject);
                return;
            }
            
            DependencyValidator.ValidateDependency(_lobbyPlayersListUpdater, nameof(_lobbyPlayersListUpdater), nameof(LobbyPlayersObserver));
            
            _players = new Dictionary<int, AbsurdRepliesPlayer>();
        }

        private void OnEnable()
        {
            if (!isClientOnly)
            {
                AbsurdRepliesPlayer.onPlayerConnected += HandlePlayerConnected;
                AbsurdRepliesPlayer.onPlayerNameChanged += HandlePlayerNameChanged;
                AbsurdRepliesNetworkManager.singleton.onClientDisconnected += HandleClientDisconnected;
            }
        }

        private void OnDisable()
        {
            if (!isClientOnly)
            {
                AbsurdRepliesPlayer.onPlayerConnected -= HandlePlayerConnected;
                AbsurdRepliesPlayer.onPlayerNameChanged -= HandlePlayerNameChanged;
            }
        }

        private void HandlePlayerConnected(AbsurdRepliesPlayer player)
        {
            Debug.Log($"Adding player {player.connectionToClient.connectionId}");
            _players[player.connectionToClient.connectionId] = player;
        }

        private void HandlePlayerNameChanged(AbsurdRepliesPlayer player)
        {
            Debug.Log($"Changing name of player {player.connectionToClient.connectionId} to {player.Name}");
            _lobbyPlayersListUpdater.UpdatePlayersText(GetPlayerNames());
        }
        
        private void HandleClientDisconnected(NetworkConnection conn)
        {
            _players.Remove(conn.connectionId);
            _lobbyPlayersListUpdater.UpdatePlayersText(GetPlayerNames());
        }

        private string[] GetPlayerNames() => _players.Values.Select(p => p.Name).ToArray();
    }
}