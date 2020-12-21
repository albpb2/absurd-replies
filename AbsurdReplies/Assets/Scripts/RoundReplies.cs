using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace AbsurdReplies
{
    public class RoundReplies : NetworkBehaviour
    {
        private Dictionary<int, string> _replies;

        private void Awake()
        {
            _replies = new Dictionary<int, string>();
        }

        [Command(ignoreAuthority = true)]
        public async void SendReply(string reply, NetworkConnectionToClient sender = null)
        {
            Debug.Log($"Received reply from {sender.connectionId}: {reply}");
            _replies[sender.connectionId] = reply;
        }
    }
}