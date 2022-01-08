using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Guilds;

public static class GuildRepositoryExtensions
{
    public static async Task<Guild> ReadForStudent(this IQueryable<GuildMember> repository, int studentId)
    {
        return await repository
            .Where(gm => gm.MemberId == studentId)
            .Where(GuildMember.IsMember())
            .Select(gm => gm.Guild)
            .SingleOrDefaultAsync();
    }
}