﻿using AbsurdReplies.Player;
using AbsurdReplies.Dependencies;
using Mirror;
using TMPro;
using UnityEngine;

namespace AbsurdReplies
{
    public class GameStarter : NetworkBehaviour
    {
        [SerializeField] private TMP_InputField _portInputField;
        [SerializeField] private TMP_InputField _playerNameField;
        
        private ushort Port => ushort.Parse(_portInputField.text);
        
        private void Awake()
        {
            DependencyValidator.ValidateDependency(_portInputField, nameof(_portInputField));
            DependencyValidator.ValidateDependency(_playerNameField, nameof(_playerNameField));
        }

        public async void StartHost()
        {
            UpdatePort();
            SetInitialPlayerName();
            AbsurdRepliesNetworkManager.singleton.StartHost();
        }

        public async void JoinLocal()
        {
            UpdatePort();
            SetInitialPlayerName();
            AbsurdRepliesNetworkManager.singleton.StartClient();
        }

        private void UpdatePort()
        {
            AbsurdRepliesNetworkManager.singleton.Transport.Port = Port;
        }

        private void SetInitialPlayerName()
        {
            PlayerName.Instance.Set(_playerNameField.text);
        }
    }
}