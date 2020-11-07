using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildTestTaskSolvingInfoRepository
    {
        GuildTestTaskSolvingInfoEntity Create(GuildEntity guild, StudentEntity student);
        IQueryable<GuildTestTaskSolvingInfoEntity> Read();
        GuildTestTaskSolvingInfoEntity Update(GuildTestTaskSolvingInfoEntity testTask);
    }
}