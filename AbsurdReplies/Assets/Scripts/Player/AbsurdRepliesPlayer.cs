using System.Threading.Tasks;
using Mirror;

namespace AbsurdReplies.Player
{
    public class AbsurdRepliesPlayer : NetworkBehaviour
    {
        public delegate void PlayerConnectedDelegate(NetworkIdentity networkIdentity);
        public static event PlayerConnectedDelegate onPlayerConnected;
        
        private async void Start()
        {
            NotifyConnected(GetComponent<NetworkIdentity>());
        }

        [Command]
        public void NotifyConnected(NetworkIdentity networkIdentity)
        {
            onPlayerConnected?.Invoke(networkIdentity);
        }
    }
}