using System;
using System.Linq.Expressions;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildMember
    {
        public GuildMember(Guild guild, IwentysUser student, GuildMemberType memberType)
            : this(guild.Id, student.Id, memberType)
        {
        }

        public GuildMember(int guildId, int studentId, GuildMemberType memberType) : this()
        {
            GuildId = guildId;
            MemberId = studentId;
            MemberType = memberType;
        }

        public GuildMember()
        {
        }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public int MemberId { get; init; }
        public virtual IwentysUser Member { get; init; }

        public GuildMemberType MemberType { get; private set; }

        public static Expression<Func<GuildMember, bool>> IsMember()
        {
            return member => member.MemberType == GuildMemberType.Creator
                             || member.MemberType == GuildMemberType.Mentor
                             || member.MemberType == GuildMemberType.Member;
        }

        public void MarkBlocked()
        {
            Member.GuildLeftTime = DateTime.UtcNow;
            MemberType = GuildMemberType.Blocked;
        }

        public void Approve(GuildMentor guildMentor)
        {
            if (MemberType != GuildMemberType.Requested)
                throw InnerLogicException.GuildExceptions.RequestWasNotFound(MemberId, GuildId);

            MemberType = GuildMemberType.Member;
        }

        public void MakeMentor(GuildCreator creator)
        {
            MemberType = GuildMemberType.Mentor;
        }

        public void Remove(GuildMentor guildMentor, IGenericRepository<GuildMember> guildMemberRepository)
        {
            if (MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.CreatorCannotLeave(MemberId, GuildId);

            guildMemberRepository.Delete(this);
        }
    }
}