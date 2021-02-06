using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildMemberListComponent
    {
        private GuildMemberLeaderBoardDto _leaderBoard;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _leaderBoard = await ClientHolder.ApiGuildMemberLeaderboardAsync(GuildProfile.Id);
        }

        private async Task KickMember(int memberId)
        {
            await ClientHolder.ApiGuildMemberKickAsync(GuildProfile.Id, memberId);
        }

        private async Task PromoteToMentor(int memberId)
        {
            await ClientHolder.ApiGuildMemberPromoteAsync(GuildProfile.Id, memberId);
        }
    }
}
