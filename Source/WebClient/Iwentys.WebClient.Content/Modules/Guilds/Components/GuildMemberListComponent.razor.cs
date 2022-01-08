using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class GuildMemberListComponent
    {
        private GuildMemberLeaderBoardDto _leaderBoard;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _leaderBoard = await _guildClient.GetGuildMemberLeaderBoardAsync(GuildProfile.Id);
        }

        private async Task KickMember(int memberId)
        {
            await _guildMembershipClient.KickMemberAsync(GuildProfile.Id, memberId);
        }

        private async Task PromoteToMentor(int memberId)
        {
            await _guildMembershipClient.PromoteToMentorAsync(GuildProfile.Id, memberId);
        }
    }
}
