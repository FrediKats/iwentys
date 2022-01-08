using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Guilds
{
    public partial class GuildMemberListComponent
    {
        private GuildMemberLeaderBoardDto _leaderBoard;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _leaderBoard = await GuildClient.GetGuildMemberLeaderBoardAsync(GuildProfile.Id);
        }

        private async Task KickMember(int memberId)
        {
            await GuildMembershipClient.KickMemberAsync(GuildProfile.Id, memberId);
        }

        private async Task PromoteToMentor(int memberId)
        {
            await GuildMembershipClient.PromoteToMentorAsync(GuildProfile.Id, memberId);
        }
    }
}
