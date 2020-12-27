using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildMemberListComponent
    {
        private GuildMemberLeaderBoardDto _leaderBoard;

        private GuildControllerClient _guildControllerClient;
        private GuildMemberControllerClient _guildMemberControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            _guildMemberControllerClient = new GuildMemberControllerClient(httpClient);
            _leaderBoard = await _guildControllerClient.GetGuildMemberLeaderBoard(GuildProfile.Id);
        }

        private async Task KickMember(int memberId)
        {
            await _guildMemberControllerClient.KickGuildMember(GuildProfile.Id, memberId);
        }

        private async Task PromoteToMentor(int memberId)
        {
            await _guildMemberControllerClient.PromoteToMentor(GuildProfile.Id, memberId);
        }
    }
}
