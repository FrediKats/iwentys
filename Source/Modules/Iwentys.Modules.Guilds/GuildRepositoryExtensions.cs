using System.Linq;
using Iwentys.Domain.Guilds;

namespace Iwentys.Modules.Guilds
{
    public static class GuildRepositoryExtensions
    {
        public static Guild ReadForStudent(this IQueryable<GuildMember> repository, int studentId)
        {
            return repository
                .Where(gm => gm.MemberId == studentId)
                .Where(GuildMember.IsMember())
                .Select(gm => gm.Guild)
                .SingleOrDefault();
        }
    }
}