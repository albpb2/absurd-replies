using kcp2k;
using Mirror;

namespace AbsurdReplies
{
    public class AbsurdRepliesNetworkManager : NetworkManager
    {
        public new static AbsurdRepliesNetworkManager singleton => NetworkManager.singleton as AbsurdRepliesNetworkManager;

        public KcpTransport Transport => Mirror.Transport.activeTransport as KcpTransport;
    }
}