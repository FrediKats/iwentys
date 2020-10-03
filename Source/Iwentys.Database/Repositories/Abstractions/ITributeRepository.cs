using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ITributeRepository : IGenericRepository<TributeEntity, long>
    {
        TributeEntity Create(GuildEntity guild, GithubProjectEntity githubProject);

        TributeEntity[] ReadForGuild(int guildId);
        TributeEntity[] ReadStudentInGuildTributes(int guildId, int studentId);
        TributeEntity ReadStudentActiveTribute(int guildId, int studentId);
    }
}