using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Features.Newsfeeds.Models;

namespace Iwentys.Endpoint.Client.Pages.Newsfeeds
{
    public partial class GuildNewsfeedCreatePage
    {
        private string _title;
        private string _description;

        private NewsfeedControllerClient _newsfeedControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _newsfeedControllerClient = new NewsfeedControllerClient(httpClient);
        }

        private async Task ExecuteCreateNewsfeed()
        {
            await _newsfeedControllerClient.CreateGuildNewsfeed(GuildId, new NewsfeedCreateViewModel
            {
                Title = _title,
                Content = _description
            });

            Navigation.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
