using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface ITournamentRepository : IGenericRepository<TournamentEntity, int>
    {
        TournamentEntity Create(TournamentEntity entity);
    }
}