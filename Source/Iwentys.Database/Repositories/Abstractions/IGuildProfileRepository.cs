using Iwentys.Database.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildProfileRepository : IGenericRepository<GuildProfile, int>
    {
        GuildProfile[] ReadPending();
    }
}