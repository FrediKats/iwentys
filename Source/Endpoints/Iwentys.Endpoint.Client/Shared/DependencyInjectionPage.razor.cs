using System.Net.Http;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Shared
{
    public partial class DependencyInjectionPage
    {
        public HttpClient Http => _httpClient;
        public ILocalStorageService LocalStorage => _localStorage;
        public NavigationManager NavigationManager => _navigationManagerClient;
    }
}
