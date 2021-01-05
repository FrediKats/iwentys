using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildMemberListComponent
    {
        private GuildMemberLeaderBoardDto _leaderBoard;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _leaderBoard = await ClientHolder.Guild.GetGuildMemberLeaderBoard(GuildProfile.Id);
        }

        private async Task KickMember(int memberId)
        {
            await ClientHolder.GuildMember.KickGuildMember(GuildProfile.Id, memberId);
        }

        private async Task PromoteToMentor(int memberId)
        {
            await ClientHolder.GuildMember.PromoteToMentor(GuildProfile.Id, memberId);
        }
    }
}
