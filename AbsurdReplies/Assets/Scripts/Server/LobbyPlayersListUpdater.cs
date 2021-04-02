using System;
using AbsurdReplies.Dependencies;
using Mirror;
using TMPro;
using UnityEngine;

namespace AbsurdReplies.Server
{
    public class LobbyPlayersListUpdater : NetworkBehaviour
    {
        [SerializeField] private TMP_Text _playersListText;

        private void Awake()
        {
            DependencyValidator.ValidateDependency(_playersListText, nameof(_playersListText));
        }
        

        [ClientRpc]
        public void UpdatePlayersText(string[] playerNames)
        {
            _playersListText.text = "Players:";
            foreach (var playerName in playerNames)
            {
                _playersListText.text += Environment.NewLine + playerName;
            }
        }
    }
}