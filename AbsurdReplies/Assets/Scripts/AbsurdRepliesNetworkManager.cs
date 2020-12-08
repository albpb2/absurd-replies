using kcp2k;
using Mirror;

namespace AbsurdReplies
{
    public class AbsurdRepliesNetworkManager : NetworkManager
    {
        public delegate void ClientDisconnectedDelegate(NetworkConnection conn);
        public event ClientDisconnectedDelegate onClientDisconnected;
        
        public new static AbsurdRepliesNetworkManager singleton => NetworkManager.singleton as AbsurdRepliesNetworkManager;

        public KcpTransport Transport => Mirror.Transport.activeTransport as KcpTransport;

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            onClientDisconnected?.Invoke(conn);
        }
    }
}