using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRepository : IGenericRepository<GuildEntity, int>
    {
        GuildEntity Create(StudentEntity creator, GuildCreateRequest arguments);

        GuildEntity[] ReadPending();
        GuildEntity ReadForStudent(int studentId);

        GuildPinnedProjectEntity PinProject(int guildId, string owner, string projectName);
        void UnpinProject(int pinnedProjectId);
    }
}