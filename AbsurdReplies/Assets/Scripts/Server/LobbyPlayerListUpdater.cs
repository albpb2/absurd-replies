using AbsurdReplies.Player;
using Mirror;
using UnityEngine;

namespace AbsurdReplies.Server
{
    public class LobbyPlayerListUpdater : NetworkBehaviour
    {
        private void Awake()
        {
            if (!isServer)
            {
                Debug.Log($"Not server, destroying {nameof(LobbyPlayerListUpdater)}");
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable()
        {
            AbsurdRepliesPlayer.onPlayerConnected += HandlePlayerConnected;
        }

        private void OnDisable()
        {
            AbsurdRepliesPlayer.onPlayerConnected += HandlePlayerConnected;
        }

        private void HandlePlayerConnected(NetworkIdentity networkIdentity)
        {
            Debug.Log($"Adding player {networkIdentity.netId}");
        }
    }
}