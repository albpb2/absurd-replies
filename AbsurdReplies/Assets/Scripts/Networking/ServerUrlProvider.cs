using System;
using AbsurdReplies.Constants;

namespace AbsurdReplies
{
    public class ServerUrlProvider
    {
        private readonly EnvironmentProvider _environmentProvider;

        public ServerUrlProvider(EnvironmentProvider environmentProvider)
        {
            _environmentProvider = environmentProvider;
        }

        public string GetServerUrl()
        {
            switch (_environmentProvider.GetEnvironment())
            {
                case Environments.Local : return ServerUrls.LocalHost;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}