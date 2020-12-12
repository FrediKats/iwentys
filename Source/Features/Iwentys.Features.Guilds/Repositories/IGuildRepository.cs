using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildRepository : IRepository<GuildEntity, int>
    {
        GuildEntity Create(StudentEntity creator, GuildCreateRequestDto arguments);
        GuildEntity[] ReadPending();
        GuildEntity ReadForStudent(int studentId);
        Task<GuildPinnedProjectEntity> PinProjectAsync(int guildId, GithubRepositoryInfoDto repositoryInfoDto);
        void UnpinProject(long pinnedProjectId);
    }
}