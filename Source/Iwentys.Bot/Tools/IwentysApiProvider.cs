using System.Net.Http;
using System.Net.Http.Headers;
using Iwentys.ApiClient.OpenAPIService;

namespace Iwentys.ClientBot.Tools
{
    public class IwentysApiProvider
    {
        private const string ServiceUrl = "http://localhost:3578";

        public Client Client { get; }

        public IwentysApiProvider()
        {
            var httpClient = new HttpClient();
            Client = new Client(ServiceUrl, httpClient);
        }

        public static Client Create(string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return new Client(ServiceUrl, httpClient);
        }
    }
}