using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models
{
    public class GuildProfileDto : GuildProfileShortInfoDto, IResultFormat
    {
        public GuildProfileDto()
        {
            
        }

        public GuildProfileDto(Guild guild) : base(guild)
        {
            Leader = guild.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new IwentysUserInfoDto(s));
            TestTasks = guild.TestTasks.SelectToList(GuildTestTaskInfoResponse.Wrap);
        }

        public static Expression<Func<Guild, GuildProfileDto>> FromEntity => entity => new GuildProfileDto(entity);

        public IwentysUserInfoDto Leader { get; set; }
        public List<GuildTestTaskInfoResponse> TestTasks { get; set; }

        public string Format()
        {
            return $"{Title}";
        }
    }
}