using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private int _readyPlayersCount;
        private bool _started;

        private async void Awake()
        {
            _random = new Random();
        }

        private async void Start()
        {
            NotifyReadyToPlayGame();
            if (isServer)
            {
                DependencyValidator.ValidateDependency(_round, nameof(_round), nameof(AbsurdRepliesGame));
            }
        }

        private async void Update()
        {
            if (isServer)
            {
                if (!_started)
                {
                    if (_readyPlayersCount >= NetworkServer.connections.Count)
                    {
                        await StartGame();
                        _started = true;
                    }
                }
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
        
        public IEnumerable<NetworkConnectionToClient> GetCurrentRoundParticipantsConnectionToClient()
        {
            for (var i = 0; i < _currentPlayerIndex; i++)
            {
                yield return _playerConnections[i];
            }

            for (var i = _currentPlayerIndex + 1; i < _playerConnections.Count; i++)
            {
                yield return _playerConnections[i];
            }
        }
        
        [Command(ignoreAuthority = true)]
        private void NotifyReadyToPlayGame()
        {
            _readyPlayersCount++;
        }
        
        private async Task StartGame()
        {
            if (!isServer)
            {
                return;
            }
            
            ShufflePlayers();
            await _round.PlayNewRound();
        }
        
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
            _currentPlayerIndex++;
            if (_currentPlayerIndex >= _playerConnections.Count)
            {
                _currentPlayerIndex = 0;
            }
            Debug.Log("Round finished");
        }
    }
}