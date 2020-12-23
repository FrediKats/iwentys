using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Guilds.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Repositories
{
    public static class GuildMemberRepositoryExtensions
    {
        public static GuildMemberEntity GetStudentMembership(this IGenericRepository<GuildMemberEntity> repository, int studentId)
        {
            //TODO: skip banned, pinned etc.
            return repository
                .GetAsync()
                .SingleOrDefault(gm => gm.MemberId == studentId);
        }
    }
}