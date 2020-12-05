using System.Linq;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildTestTaskSolvingInfoRepository
    {
        GuildTestTaskSolvingInfoEntity Create(GuildEntity guild, StudentEntity student);
        IQueryable<GuildTestTaskSolvingInfoEntity> Read();
        GuildTestTaskSolvingInfoEntity Update(GuildTestTaskSolvingInfoEntity testTask);
    }
}