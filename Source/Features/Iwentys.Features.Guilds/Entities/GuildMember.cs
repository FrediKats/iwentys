using System;
using System.Linq.Expressions;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildMember
    {
        protected GuildMember()
        {
        }

        public GuildMember(Guild guild, Student student, GuildMemberType memberType)
            : this(guild.Id, student.Id, memberType)
        {
        }

        public GuildMember(int guildId, int studentId, GuildMemberType memberType) : this()
        {
            GuildId = guildId;
            MemberId = studentId;
            MemberType = memberType;
        }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int MemberId { get; set; }
        public virtual Student Member { get; set; }

        public GuildMemberType MemberType { get; set; }
        
        public void MarkBlocked()
        {
            Member.GuildLeftTime = DateTime.UtcNow;
            MemberType = GuildMemberType.Blocked;
        }

        public static Expression<Func<GuildMember, bool>> IsMember()
        {
            return member => member.MemberType == GuildMemberType.Creator
                             || member.MemberType == GuildMemberType.Mentor
                             || member.MemberType == GuildMemberType.Member;
        }
    }
}