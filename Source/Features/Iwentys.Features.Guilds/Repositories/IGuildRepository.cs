using Iwentys.Common.Databases;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildRepository : IRepository<GuildEntity, int>
    {
        GuildEntity ReadForStudent(int studentId);
    }
}