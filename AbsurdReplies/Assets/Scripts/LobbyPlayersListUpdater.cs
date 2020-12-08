using System;
using System.Collections.Generic;
using AbsurdReplies.Server;
using Mirror;
using TMPro;
using UnityEngine;

namespace AbsurdReplies
{
    public class LobbyPlayersListUpdater : NetworkBehaviour
    {
        [SerializeField] private TMP_Text _playersListText;

        private void Awake()
        {
            DependencyValidator.ValidateDependency(_playersListText, nameof(_playersListText), nameof(LobbyPlayersObserver));
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