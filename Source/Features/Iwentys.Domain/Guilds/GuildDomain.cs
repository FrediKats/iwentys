using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Enums;

namespace Iwentys.Domain.Guilds
{
    public class GuildDomain
    {
        private readonly IGenericRepository<IwentysUser> _userRepository;

        public GuildDomain(
            Guild profile,
            IGenericRepository<IwentysUser> studentRepository)
        {
            Profile = profile;
            _userRepository = studentRepository;
        }

        public Guild Profile { get; }

        

        public GuildMember EnsureMemberCanRestrictPermissionForOther(IwentysUser editorStudentAccount, int memberToKickId)
        {
            editorStudentAccount.EnsureIsGuildMentor(Profile);

            GuildMember memberToKick = Profile.Members.Find(m => m.MemberId == memberToKickId);
            GuildMember editorMember = Profile.Members.Find(m => m.MemberId == editorStudentAccount.Id) ?? throw new EntityNotFoundException(nameof(GuildMember));

            //TODO: check
            //if (memberToKick is null || !memberToKick.MemberType.IsMember())
            if (memberToKick is null)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(editorStudentAccount.Id, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Mentor && editorMember.MemberType == GuildMemberType.Mentor)
                throw InnerLogicException.GuildExceptions.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            return memberToKick;
        }
    }
}