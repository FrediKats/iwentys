using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Integrations.GithubIntegration.Models;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildRepository : IGenericRepository<GuildEntity, int>
    {
        GuildEntity Create(StudentEntity creator, GuildCreateRequest arguments);
        GuildEntity[] ReadPending();
        GuildEntity ReadForStudent(int studentId);
        Task<GuildPinnedProjectEntity> PinProjectAsync(int guildId, GithubRepository repository);
        void UnpinProject(long pinnedProjectId);
    }
}