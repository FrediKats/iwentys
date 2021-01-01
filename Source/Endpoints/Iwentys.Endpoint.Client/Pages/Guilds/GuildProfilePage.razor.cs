using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Newsfeeds.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage
    {
        private ExtendedGuildProfileWithMemberDataDto _guild;
        private GuildMemberLeaderBoardDto _memberLeaderBoard;
        private List<NewsfeedViewModel> _newsfeeds;
        private TributeInfoResponse _activeTribute;
        private TournamentInfoResponse _activeTournament;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _guild = await ClientHolder.Guild.Get(GuildId);
            _newsfeeds = await ClientHolder.Newsfeed.GetForGuild(GuildId);
            _memberLeaderBoard = await ClientHolder.Guild.GetGuildMemberLeaderBoard(_guild.Id);
            _activeTribute = await ClientHolder.GuildTribute.FindStudentActiveTribute();
            _activeTournament = await ClientHolder.Tournament.FindGuildActiveTournament(_guild.Id);
        }

        private string LinkToCreateNewsfeedPage()
        {
            return $"/newsfeed/create-guild/{GuildId}";
        }
    }
}
