using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildProfileShortInfoWithLeaderDto : GuildProfileShortInfoDto, IResultFormat
    {
        public GuildProfileShortInfoWithLeaderDto(GuildEntity guild) : base(guild)
        {
            Leader = guild.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentInfoDto(s));
        }

        public StudentInfoDto Leader { get; set; }

        public string Format()
        {
            return $"{Title}";
        }
    }
}