using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AbsurdReplies
{
    public static class HttpClientFactory
    {
        private static object _httpClientLock = new object();
        private static HttpClient _httpClient;

        private static readonly EnvironmentProvider _environmentProvider = new EnvironmentProvider();
        
        static HttpClientFactory()
        {
            if (_environmentProvider.GetEnvironment() == Environments.Local)
            {
                // TODO: Remove this temporary dirty trick to skip local cert validation
                ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
            }
        }
        
        public static HttpClient GetHttpClient()
        {
            if (_httpClient == null)
            {
                lock (_httpClientLock)
                {
                    if (_httpClient == null)
                    {
                        _httpClient = new HttpClient();
                    }
                }
            }

            return _httpClient;
        }

        private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            // all Certificates are accepted
            return true;
        }
    }
}