using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Newsfeeds.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage : ComponentBase
    {
        private ExtendedGuildProfileWithMemberDataDto _guild;
        private GuildMemberLeaderBoardDto _memberLeaderBoard;
        private List<NewsfeedViewModel> _newsfeeds;
        private TributeInfoResponse _activeTribute;
        private TournamentInfoResponse _activeTournament;
        
        private GuildControllerClient _guildControllerClient;
        private GuildTributeControllerClient _guildTributeControllerClient;
        private TournamentControllerClient _tournamentControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            _guildTributeControllerClient = new GuildTributeControllerClient(httpClient);
            _tournamentControllerClient = new TournamentControllerClient(httpClient);

            var newsfeedControllerClient = new NewsfeedControllerClient(httpClient);
            _guild = await _guildControllerClient.Get(GuildId);
            _newsfeeds = await newsfeedControllerClient.GetForGuild(GuildId);
            _memberLeaderBoard = await _guildControllerClient.GetGuildMemberLeaderBoard(_guild.Id);
            _activeTribute = await _guildTributeControllerClient.FindStudentActiveTribute();
            _activeTournament = await _tournamentControllerClient.FindGuildActiveTournament(_guild.Id);
        }
    }
}
