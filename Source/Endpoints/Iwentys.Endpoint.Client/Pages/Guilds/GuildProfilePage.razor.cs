using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Newsfeeds.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage : ComponentBase
    {
        private ExtendedGuildProfileWithMemberDataDto _guild;
        private GuildMemberLeaderBoardDto _memberLeaderBoard;
        private List<NewsfeedViewModel> _newsfeeds;

        private GuildControllerClient _guildControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            var newsfeedControllerClient = new NewsfeedControllerClient(httpClient);
            _guild = await _guildControllerClient.Get(GuildId);
            _newsfeeds = await newsfeedControllerClient.GetForGuild(GuildId);
            _memberLeaderBoard = await _guildControllerClient.GetGuildMemberLeaderBoard(_guild.Id);
        }
    }
}
