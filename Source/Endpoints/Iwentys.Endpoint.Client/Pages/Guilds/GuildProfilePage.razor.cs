using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage
    {
        private GuildProfileDto _guild;
        private GuildMemberLeaderBoardDto _memberLeaderBoard;
        private List<NewsfeedViewModel> _newsfeeds;
        private List<AchievementInfoDto> _achievements;
        private TributeInfoResponse _activeTribute;
        private TournamentInfoResponse _activeTournament;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _guild = await GuildClient.GetAsync(GuildId);
            _newsfeeds = (await NewsfeedClient.GetByGuildIdAsync(GuildId)).ToList();
            _memberLeaderBoard = await GuildClient.GetGuildMemberLeaderBoardAsync(_guild.Id);

            try
            {
                _activeTribute = await GuildTributeClient.FindStudentActiveTributeAsync();
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                _logger.Log(LogLevel.Error, e, "Failed to fetch member tribute.");
            }

            try
            {
                _activeTournament = await TournamentClient.FindActiveByGuildIdAsync(_guild.Id);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, $"Failed to fetch active tournament. GuildId: {_guild.Id}");
            }

            
            _achievements = (await AchievementClient.GetByGuildIdAsync(GuildId)).ToList();
        }

        private string LinkToCreateNewsfeedPage()
        {
            return $"/newsfeed/create-guild/{GuildId}";
        }
    }
}
