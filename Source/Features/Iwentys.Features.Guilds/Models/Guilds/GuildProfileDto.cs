using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildProfileDto : GuildProfileShortInfoDto, IResultFormat
    {
        public GuildProfileDto()
        {
            
        }
        public GuildProfileDto(GuildEntity guild) : base(guild)
        {
            Leader = guild.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentInfoDto(s));
            Achievements = guild.Achievements.SelectToList(AchievementDto.Wrap);
            TestTasks = guild.TestTasks.SelectToList(GuildTestTaskInfoResponse.Wrap);
        }

        public static Expression<Func<GuildEntity, GuildProfileDto>> FromEntity => entity => new GuildProfileDto(entity);

        public StudentInfoDto Leader { get; set; }
        public List<AchievementDto> Achievements { get; set; }
        public List<GuildTestTaskInfoResponse> TestTasks { get; set; }

        public string Format()
        {
            return $"{Title}";
        }
    }
}