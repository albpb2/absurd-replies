using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Zenject;

namespace AbsurdReplies
{
    public class RoundReplies : NetworkBehaviour
    {
        private AbsurdRepliesGame _game;
        
        private Dictionary<int, string> _replies;
        private string _trueReply;

        [Inject]
        public void InitializeDependencies(AbsurdRepliesGame game)
        {
            _game = game;
            
            DependencyValidator.ValidateDependency(game, nameof(game), nameof(RoundReplies));
        }

        private void Awake()
        {
            if (!isServer)
                return;

            _replies = new Dictionary<int, string>();
        }

        [Command(ignoreAuthority = true)]
        public async void SendReply(string reply, NetworkConnectionToClient sender = null)
        {
            Debug.Log($"Received reply from {sender.connectionId}: {reply}");
            _replies[sender.connectionId] = reply;
        }

        public void SetTrueReply(string reply)
        {
            _trueReply = reply;
        }
    }
}