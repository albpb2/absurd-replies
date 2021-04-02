using System.Collections.Generic;
using AbsurdReplies.Dependencies;
using AbsurdReplies.Exceptions;
using Mirror;
using UnityEngine;
using Zenject;

namespace AbsurdReplies
{
    public class RoundReplies : NetworkBehaviour
    {
        private AbsurdRepliesGame _game;
        
        private Dictionary<int, string> _repliesByConnectionId;
        private string _trueReply;

        public Dictionary<int, string> RepliesByConnectionId => _repliesByConnectionId;

        [Inject]
        public void InitializeDependencies(AbsurdRepliesGame game)
        {
            _game = game ?? throw ExceptionBecause.MissingDependency(nameof(game));
        }

        private void Awake()
        {
            if (!isServer)
                return;

            _repliesByConnectionId = new Dictionary<int, string>();
        }

        public void Initialize()
        {
            _repliesByConnectionId = new Dictionary<int, string>();
        }

        [Command(ignoreAuthority = true)]
        public async void SendReply(string reply, NetworkConnectionToClient sender = null)
        {
            Debug.Log($"Received reply from {sender.connectionId}: {reply}");
            _repliesByConnectionId[sender.connectionId] = reply;
        }

        public void SetTrueReply(string reply)
        {
            _trueReply = reply;
            _repliesByConnectionId[_game.GetCurrentRoundLeaderConnectionToClient().connectionId] = reply;
        }
    }
}