using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Repositories
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