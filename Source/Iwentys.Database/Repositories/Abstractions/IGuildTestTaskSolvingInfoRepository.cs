using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildTestTaskSolvingInfoRepository
    {
        GuildTestTaskSolvingInfoEntity Create(GuildEntity guild, Student student);
        IQueryable<GuildTestTaskSolvingInfoEntity> Read();
    }
}