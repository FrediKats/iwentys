using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildProfileDto : GuildProfileShortInfoWithLeaderDto
    {
        public GuildProfileDto(GuildEntity guild) : base(guild)
        {
            Achievements = guild.Achievements.SelectToList(AchievementDto.Wrap);
            TestTasks = guild.TestTasks.SelectToList(GuildTestTaskInfoResponse.Wrap);
        }


        public List<AchievementDto> Achievements { get; set; }
        public List<GuildTestTaskInfoResponse> TestTasks { get; set; }
    }
}