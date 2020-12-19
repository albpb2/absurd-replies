using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Random = System.Random;

namespace AbsurdReplies
{
    public class AbsurdRepliesGame : NetworkBehaviour
    {
        [SerializeField] private AbsurdRepliesRound _round;
        
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
                DependencyValidator.ValidateDependency(_round, nameof(_round), nameof(AbsurdRepliesGame));
                ShufflePlayers();
                await _round.PlayNewRound();
            }
        }

        private void OnEnable()
        {
            _round.onRoundFinished += HandleRoundFinished;
        }

        private void OnDisable()
        {
            _round.onRoundFinished -= HandleRoundFinished;
        }

        public NetworkConnectionToClient GetCurrentRoundLeaderConnectionToClient() => _playerConnections[_currentPlayerIndex];

        private void ShufflePlayers()
        {
            _playerConnections = NetworkServer.connections.Values.ToList();
            _playerConnections.Shuffle(_random);

            var shuffledPlayersLog = "Players shuffled: ";
            for (var i = 0; i < _playerConnections.Count; i++)
            {
                shuffledPlayersLog += $"{_playerConnections[i].connectionId}, ";
            }

            shuffledPlayersLog = shuffledPlayersLog.Substring(0, shuffledPlayersLog.Length - 2);
            
            Debug.Log(shuffledPlayersLog);
        }

        private void HandleRoundFinished()
        {
            Debug.Log("Round finished");
        }
    }
}