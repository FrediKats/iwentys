using System;
using System.Linq.Expressions;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Enums;

namespace Iwentys.Domain.Guilds
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
        public int MemberImpact { get; set; }

        public static Expression<Func<GuildMember, bool>> IsMember()
        {
            return member => member.MemberType == GuildMemberType.Creator
                             || member.MemberType == GuildMemberType.Mentor
                             || member.MemberType == GuildMemberType.Member;
        }

        public void MarkBlocked(GuildLastLeave guildLastLeave)
        {
            guildLastLeave.UpdateLeave();
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
            if (MemberType != GuildMemberType.Member)
                throw new InnerLogicException("Cant make mentor");
            MemberType = GuildMemberType.Mentor;
        }

        public void Remove(GuildMentor guildMentor, IGenericRepository<GuildMember> guildMemberRepository, GuildLastLeave guildLastLeave)
        {
            if (MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.CreatorCannotLeave(MemberId, GuildId);

            //TODO: do not remove, mark as deleted
            guildLastLeave.UpdateLeave();
            guildMemberRepository.Delete(this);
        }
    }
}