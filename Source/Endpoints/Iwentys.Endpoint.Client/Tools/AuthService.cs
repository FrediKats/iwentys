using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Tools
{
    public interface IAuthService
    {
        Task Login(int userId);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task Login(int userId)
        {
            IwentysAuthResponse iwentysAuthResponse = await new AuthControllerClient(_httpClient).Login(userId);
            await _localStorage.SetItemAsync("authToken", iwentysAuthResponse.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", iwentysAuthResponse.Token);
        }
    }
}