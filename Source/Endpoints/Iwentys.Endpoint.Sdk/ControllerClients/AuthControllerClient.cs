using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Common.Transferable;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class AuthControllerClient
    {
        public AuthControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<IwentysAuthResponse> Login(int id)
        {
            return Client.GetFromJsonAsync<IwentysAuthResponse>($"api/isuauth/login/{id}");
        }
    }
}