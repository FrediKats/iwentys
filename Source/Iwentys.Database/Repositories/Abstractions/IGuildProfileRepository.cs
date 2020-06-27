using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildProfileRepository : IGenericRepository<GuildProfile, int>
    {
        GuildProfile[] ReadPending();
        GuildProfile ReadForUser(int userId);
    }
}