using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ITributeRepository : IGenericRepository<Tribute, int>
    {
        Tribute[] ReadForGuild(int guildId);
        Tribute[] ReadStudentInGuildTributes(int guildId, int studentId);
        Tribute ReadStudentActiveTribute(int guildId, int studentId);
    }
}