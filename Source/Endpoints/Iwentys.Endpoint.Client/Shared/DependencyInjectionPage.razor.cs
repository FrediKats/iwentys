using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Client.Shared
{
    public partial class DependencyInjectionPage
    {
        //public HttpClient Http => _httpClient;
        public ILocalStorageService LocalStorage => _localStorage;
        public NavigationManager NavigationManager => _navigationManagerClient;
        public ILogger Logger => _logger;

        public ControllerClientHolder ClientHolder { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await _httpClient.TrySetHeader(_localStorage);
            ClientHolder = new ControllerClientHolder(httpClient);
        }
    }
}
