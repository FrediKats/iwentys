using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Iwentys.Endpoint.Client.Tools
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpClient> TrySetHeader(this HttpClient client, ILocalStorageService localStorage)
        {
            var savedToken = await localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(savedToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}