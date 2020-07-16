using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRepository : IGenericRepository<Guild, int>
    {
        Guild[] ReadPending();
        Guild ReadForStudent(int studentId);
        Guild ReadForTotem(int totemId);

        void RemoveMember(int guildId, int userId);
    }
}