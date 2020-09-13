using System;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRepository : IGenericRepository<GuildEntity, int>
    {
        GuildEntity[] ReadPending();
        GuildEntity ReadForStudent(int studentId);

        // TODO: extract methods below to GuildMemberRepository
        Boolean IsStudentHaveRequest(int studentId);

        GuildMember AddMember(GuildMember member);
        GuildMember UpdateMember(GuildMember member);
        void RemoveMember(int guildId, int userId);

        GuildPinnedProject PinProject(int guildId, string owner, string projectName);
        void UnpinProject(int pinnedProjectId);
    }
}