using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildTributeRepository : IGenericRepository<TributeEntity, long>
    {
        TributeEntity Create(GuildEntity guild, GithubProjectEntity githubProject);
        TributeEntity[] ReadForGuild(int guildId);
        TributeEntity[] ReadStudentInGuildTributes(int guildId, int studentId);
        TributeEntity ReadStudentActiveTribute(int guildId, int studentId);
    }
}