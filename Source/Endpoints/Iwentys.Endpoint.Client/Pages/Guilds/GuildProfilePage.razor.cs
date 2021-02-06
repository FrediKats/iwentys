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

            _guild = await ClientHolder.Guild.Get(GuildId);
            _newsfeeds = await ClientHolder.ApiNewsfeedGuildGetAsync(GuildId);
            _memberLeaderBoard = await ClientHolder.Guild.GetGuildMemberLeaderBoard(_guild.Id);
            _activeTribute = await ClientHolder.GuildTribute.FindStudentActiveTribute();
            _activeTournament = await ClientHolder.Tournament.FindGuildActiveTournament(_guild.Id);
            _achievements = await ClientHolder.Achievement.GetForGuild(GuildId);
        }

        private string LinkToCreateNewsfeedPage()
        {
            return $"/newsfeed/create-guild/{GuildId}";
        }
    }
}
