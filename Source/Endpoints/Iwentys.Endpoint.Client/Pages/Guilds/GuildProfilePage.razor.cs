using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.Newsfeeds.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage : ComponentBase
    {
        private GuildProfileDto _guild;
        private List<NewsfeedViewModel> _newsfeeds;

        private GuildControllerClient _guildControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            var newsfeedControllerClient = new NewsfeedControllerClient(httpClient);
            _guild = await _guildControllerClient.Get(GuildId);
            _newsfeeds = await newsfeedControllerClient.GetForGuild(GuildId);
        }
    }
}
