using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRepository : IGenericRepository<Guild, int>
    {
        Guild[] ReadPending();
        Guild ReadForUser(int userId);
    }
}