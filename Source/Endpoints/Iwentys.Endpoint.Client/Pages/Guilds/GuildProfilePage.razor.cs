using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.Newsfeeds.ViewModels;
using Iwentys.Models.Transferable.Study;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage : ComponentBase
    {
        private GuildProfileDto _guild;
        private List<NewsfeedInfoResponse> _newsfeeds;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            var guildControllerClient = new GuildControllerClient(httpClient);
            var newsfeedControllerClient = new NewsfeedControllerClient(httpClient);
            _guild = await guildControllerClient.Get(GuildId);
            _newsfeeds = await newsfeedControllerClient.GetForGuild(GuildId);
        }
    }
}
