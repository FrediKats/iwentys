using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Models.Guilds;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildMemberListComponent
    {
        private GuildMemberLeaderBoardDto _leaderBoard;

        private GuildControllerClient _guildControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            _leaderBoard = await _guildControllerClient.GetGuildMemberLeaderBoard(GuildProfile.Id);
        }
    }
}
