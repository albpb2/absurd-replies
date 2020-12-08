using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;

namespace AbsurdReplies
{
    public class AbsurdRepliesGame : NetworkBehaviour
    {
        [SyncVar] private int _currentPlayerIndex;

        private Random _random;
        
        // Only in server
        private List<NetworkConnectionToClient> _playerConnections;

        private async void Awake()
        {
            _random = new Random();
        }

        private async void Start()
        {
            if (isServer)
            {
                _playerConnections = NetworkServer.connections.Values.ToList();
                _playerConnections.Shuffle(_random);
            }
        }
    }
}