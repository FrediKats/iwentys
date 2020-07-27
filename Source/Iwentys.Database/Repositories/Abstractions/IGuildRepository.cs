using System;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRepository : IGenericRepository<Guild, int>
    {
        Guild[] ReadPending();
        Guild ReadForStudent(int studentId);
        Guild ReadForTotem(int totemId);

        Boolean IsStudentHaveRequest(int studentId);

        void AddMember(int guildId, int userId);
        void AddRequest(int guildId, int userId);
        void RemoveMember(int guildId, int userId);
    }
}