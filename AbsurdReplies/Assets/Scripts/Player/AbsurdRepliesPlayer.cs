using Mirror;

namespace AbsurdReplies.Player
{
    public class AbsurdRepliesPlayer : NetworkBehaviour
    {
        public delegate void PlayerConnectedDelegate(AbsurdRepliesPlayer player);
        public delegate void PlayerNameChangedDelegate(AbsurdRepliesPlayer player);
        public static event PlayerConnectedDelegate onPlayerConnected;
        public static event PlayerNameChangedDelegate onPlayerNameChanged;

        [SyncVar] private string _name;

        public string Name => _name;
        
        private async void Start()
        {
            if (isLocalPlayer)
            {
                NotifyConnected();
                SetName(PlayerName.Instance.Get());
            }
        }

        [Command]
        private void NotifyConnected()
        {
            onPlayerConnected?.Invoke(this);
        }

        [Command]
        private void SetName(string playerName)
        {
            _name = playerName;
            onPlayerNameChanged?.Invoke(this);
        }
    }
}