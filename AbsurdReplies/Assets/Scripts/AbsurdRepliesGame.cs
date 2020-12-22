using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
using Mirror;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace AbsurdReplies
{
    public class AbsurdRepliesGame : NetworkBehaviour
    {
        [SyncVar] private int _currentPlayerIndex;

        private AbsurdRepliesRound _round;

        // Only in server
        private List<NetworkConnectionToClient> _playerConnections;
        private int _readyPlayersCount;
        private bool _started;

        [Inject]
        public void InitializeDependencies(AbsurdRepliesRound round)
        {
            _round = round;

            DependencyValidator.ValidateDependency(_round, nameof(_round), nameof(AbsurdRepliesGame));
        }

        private async void Start()
        {
            NotifyReadyToPlayGame();
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
            _playerConnections.Shuffle();

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
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _playerConnections.Count;
            
            Debug.Log("Round finished");
        }
    }
}