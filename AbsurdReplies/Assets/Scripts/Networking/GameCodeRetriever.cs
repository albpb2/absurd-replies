using System.Threading.Tasks;

namespace AbsurdReplies
{
    public class GameCodeRetriever
    {
        private readonly ServerUrlProvider _serverUrlProvider;
        
        public GameCodeRetriever(ServerUrlProvider serverUrlProvider)
        {
            _serverUrlProvider = serverUrlProvider;
        }

        public async Task<string> RetrieveGameCode()
        {
            var httpClient = HttpClientFactory.GetHttpClient(); 
            var response = await httpClient.GetAsync(GameCodeEndpointUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private string GameCodeEndpointUrl => $"{_serverUrlProvider.GetServerUrl()}/GameCode";
    }
}