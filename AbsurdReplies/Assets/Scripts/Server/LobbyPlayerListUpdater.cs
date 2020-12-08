using System.Collections.Generic;
using AbsurdReplies.Player;
using Mirror;
using UnityEngine;

namespace AbsurdReplies.Server
{
    public class LobbyPlayerListUpdater : NetworkBehaviour
    {
        private List<AbsurdRepliesPlayer> _players;
        
        private void Awake()
        {
            if (isClientOnly)
            {
                Debug.Log($"Not server, destroying {nameof(LobbyPlayerListUpdater)}");
                Destroy(gameObject);
                return;
            }
            
            _players = new List<AbsurdRepliesPlayer>();
        }

        private void OnEnable()
        {
            if (!isClientOnly)
            {
                AbsurdRepliesPlayer.onPlayerConnected += HandlePlayerConnected;
                AbsurdRepliesPlayer.onPlayerNameChanged += HandlePlayerNameChanged;
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
            Debug.Log($"Adding player {player.connectionToClient.identity.netId}");
            _players.Add(player);
        }

        private void HandlePlayerNameChanged(AbsurdRepliesPlayer player)
        {
            Debug.Log($"Changing name of player {player.connectionToClient.identity.netId} to {player.Name}");
        }
    }
}