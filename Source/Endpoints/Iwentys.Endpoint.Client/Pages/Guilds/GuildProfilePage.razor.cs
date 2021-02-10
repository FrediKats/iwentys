using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage
    {
        private GuildProfileDto _guild;
        private GuildMemberLeaderBoardDto _memberLeaderBoard;
        private ICollection<NewsfeedViewModel> _newsfeeds;
        private ICollection<AchievementInfoDto> _achievements;
        private TributeInfoResponse _activeTribute;
        private TournamentInfoResponse _activeTournament;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _guild = await ClientHolder.ApiGuildGetAsync(GuildId);
            _newsfeeds = await ClientHolder.ApiNewsfeedGuildGetAsync(GuildId);
            _memberLeaderBoard = await ClientHolder.ApiGuildMemberLeaderboardAsync(_guild.Id);
            _activeTribute = await ClientHolder.ApiGuildTributeGetForStudentActiveAsync();
            _activeTournament = await ClientHolder.ApiTournamentsForGuildAsync(_guild.Id);
            _achievements = await ClientHolder.ApiAchievementsGuildsAsync(GuildId);
        }

        private string LinkToCreateNewsfeedPage()
        {
            return $"/newsfeed/create-guild/{GuildId}";
        }
    }
}
