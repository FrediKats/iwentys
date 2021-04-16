using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Guilds;

namespace Iwentys.Domain.Services
{
    public static class GuildMemberRepositoryExtensions
    {
        public static GuildMember GetStudentMembership(this IGenericRepository<GuildMember> repository, int studentId)
        {
            return repository
                .Get()
                .Where(GuildMember.IsMember())
                .SingleOrDefault(gm => gm.MemberId == studentId);
        }
    }
}