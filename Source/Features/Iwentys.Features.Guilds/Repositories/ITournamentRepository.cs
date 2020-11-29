using Iwentys.Common.Tools;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface ITournamentRepository : IGenericRepository<TournamentEntity, int>
    {
        TournamentEntity Create(TournamentEntity entity);
    }
}