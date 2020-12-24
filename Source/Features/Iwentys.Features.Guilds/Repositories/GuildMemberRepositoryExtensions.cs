using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public static class GuildMemberRepositoryExtensions
    {
        public static GuildMemberEntity GetStudentMembership(this IGenericRepository<GuildMemberEntity> repository, int studentId)
        {
            return repository
                .GetAsync()
                .Where(GuildMemberEntity.IsMember())
                .SingleOrDefault(gm => gm.MemberId == studentId);
        }
    }
}