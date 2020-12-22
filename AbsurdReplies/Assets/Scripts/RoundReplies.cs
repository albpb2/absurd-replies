using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace AbsurdReplies
{
    public class RoundReplies : NetworkBehaviour
    {
        [SerializeField] private AbsurdRepliesGame _game;
        
        private Dictionary<int, string> _replies;
        private string _trueReply;

        private void Awake()
        {
            if (!isServer)
                return;
            
            DependencyValidator.ValidateDependency(_game, nameof(_game), nameof(RoundReplies));
            
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