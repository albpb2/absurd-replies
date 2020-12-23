using System;
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
        private readonly string[] _options = {"A", "B", "C", "D", "E", "F", "G", "H"};
        
        private VotingView _votingView;
        private RoundReplies _roundReplies;
        
        private Dictionary<string, int> _connectionIdByOption;
        private Dictionary<int, int> _voteByConnectionId;

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

        public Task StartProcess()
        {
            Initialize();
            AssignShuffledOptions();
            ShowReplies();

            return Task.CompletedTask;
        }

        [Command(ignoreAuthority = true)]
        public async void Vote(string option, NetworkConnectionToClient sender = null)
        {
            var votedPlayer = _connectionIdByOption[option];
            
            Debug.Log($"{sender.connectionId} voted for {votedPlayer}");

            _voteByConnectionId[sender.connectionId] = votedPlayer;
        }

        private void ShowReplies()
        {
            var repliesByOption = new Dictionary<string, string>();
            foreach (var option in _connectionIdByOption.Keys)
            {
                repliesByOption[option] = _roundReplies.RepliesByConnectionId[_connectionIdByOption[option]];
            }
            
            ShowReplies(FormatOptionsAndRepliesText(repliesByOption));
        }

        [ClientRpc]
        private async void ShowReplies(string repliesText)
        {
            _votingView.DisplayOptionsAndReplies(repliesText);
        }

        private string FormatOptionsAndRepliesText(Dictionary<string, string> repliesByOption)
        {
            var text = string.Empty;
            foreach (var option in repliesByOption.Keys.OrderBy(k => k))
            {
                text += $"{option}: {repliesByOption[option]}{Environment.NewLine}";
            }

            return text;
        }

        private void Initialize()
        {
            _connectionIdByOption = new Dictionary<string, int>();
            _voteByConnectionId = new Dictionary<int, int>();
        }

        private void AssignShuffledOptions()
        {
            if (_roundReplies.RepliesByConnectionId == null)
            {
                return;
            }
            
            var connectionIds = _roundReplies.RepliesByConnectionId.Keys.ToList();
            connectionIds.Shuffle();

            for (var i = 0; i < connectionIds.Count; i++)
            {
                _connectionIdByOption[_options[i]] = connectionIds[i];
            }
            
            SetValidOptions(_connectionIdByOption.Keys.ToArray());
        }

        [ClientRpc]
        public async void SetValidOptions(string[] validOptions)
        {
            _votingView.SetValidOptions(validOptions);
        }
    }
}