using System;
using System.Linq.Expressions;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildMemberEntity
    {
        protected GuildMemberEntity()
        {
        }

        public GuildMemberEntity(GuildEntity guild, StudentEntity student, GuildMemberType memberType)
            : this(guild.Id, student.Id, memberType)
        {
        }

        public GuildMemberEntity(int guildId, int studentId, GuildMemberType memberType) : this()
        {
            GuildId = guildId;
            MemberId = studentId;
            MemberType = memberType;
        }

        public int GuildId { get; set; }
        public virtual GuildEntity Guild { get; set; }

        public int MemberId { get; set; }
        public virtual StudentEntity Member { get; set; }

        public GuildMemberType MemberType { get; set; }
        
        public void MarkBlocked()
        {
            Member.GuildLeftTime = DateTime.UtcNow;
            MemberType = GuildMemberType.Blocked;
        }

        public static Expression<Func<GuildMemberEntity, bool>> IsMember()
        {
            return member => member.MemberType == GuildMemberType.Creator
                             || member.MemberType == GuildMemberType.Mentor
                             || member.MemberType == GuildMemberType.Member;
        }
    }
}