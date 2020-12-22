using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
using Mirror;
using UnityEngine;
using Zenject;

namespace AbsurdReplies
{
    public class VotingProcess : NetworkBehaviour
    {
        private VotingView _votingView;
        private RoundReplies _roundReplies;
        
        private Dictionary<string, int> _connectionIdByOption;
        private Dictionary<int, int> _voteByConnectionId;

        private readonly string[] Options = new[] {"A", "B", "C", "D", "E", "F", "G", "H"};

        [Inject]
        public void InitializeDependencies(VotingView votingView, RoundReplies roundReplies)
        {
            _votingView = votingView;
            _roundReplies = roundReplies;
            
            DependencyValidator.ValidateDependency(_votingView, nameof(_votingView), nameof(VotingProcess));
            DependencyValidator.ValidateDependency(_roundReplies, nameof(_roundReplies), nameof(VotingProcess));
        }

        private async void Awake()
        {
            Initialize();
        }

        public void StartProcess()
        {
            Initialize();
            AssignShuffledOptions();
            ShowReplies();
        }

        [Command(ignoreAuthority = true)]
        public async void Vote(string option, NetworkConnectionToClient sender = null)
        {
            var votedPlayer = _connectionIdByOption[option];
            
            Debug.Log($"{sender.connectionId} voted for {votedPlayer}");

            _voteByConnectionId[sender.connectionId] = votedPlayer;
        }

        public Task ShowReplies()
        {
            var repliesByOption = new Dictionary<string, string>();
            foreach (var option in _connectionIdByOption.Keys)
            {
                repliesByOption[option] = _roundReplies.RepliesByConnectionId[_connectionIdByOption[option]];
            }
            
            return _votingView.DisplayOptionsAndReplies(repliesByOption);
        }

        private void Initialize()
        {
            _connectionIdByOption = new Dictionary<string, int>();
            _voteByConnectionId = new Dictionary<int, int>();
        }

        private void AssignShuffledOptions()
        {
            var connectionIds = _roundReplies.RepliesByConnectionId.Keys.ToList();
            connectionIds.Shuffle();

            for (var i = 0; i < connectionIds.Count; i++)
            {
                _connectionIdByOption[Options[i]] = connectionIds[i];
            }
            
            _votingView.SetValidOptions(_connectionIdByOption.Keys);
        }
    }
}