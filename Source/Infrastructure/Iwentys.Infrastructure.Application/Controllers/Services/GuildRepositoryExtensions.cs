using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Guilds;

namespace Iwentys.Infrastructure.Application.Controllers.Services
{
    public static class GuildRepositoryExtensions
    {
        public static Guild ReadForStudent(this IGenericRepository<GuildMember> repository, int studentId)
        {
            return repository
                .Get()
                .Where(gm => gm.MemberId == studentId)
                .Where(GuildMember.IsMember())
                .Select(gm => gm.Guild)
                .SingleOrDefault();
        }
    }
}