using System;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Domain;
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

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public int MemberId { get; init; }
        public virtual Student Member { get; init; }

        public GuildMemberType MemberType { get; private set; }
        
        public void MarkBlocked()
        {
            Member.GuildLeftTime = DateTime.UtcNow;
            MemberType = GuildMemberType.Blocked;
        }

        public void Approve(GuildEditor guildEditor)
        {
            if (MemberType != GuildMemberType.Requested)
                throw InnerLogicException.GuildExceptions.RequestWasNotFound(MemberId, GuildId);

            MemberType = GuildMemberType.Member;
        }

        public void MakeMentor(GuildCreator creator)
        {
            MemberType = GuildMemberType.Mentor;
        }

        public static Expression<Func<GuildMember, bool>> IsMember()
        {
            return member => member.MemberType == GuildMemberType.Creator
                             || member.MemberType == GuildMemberType.Mentor
                             || member.MemberType == GuildMemberType.Member;
        }
    }
}