using System;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRepository : IGenericRepository<GuildEntity, int>
    {
        GuildEntity Create(Student creator, GuildCreateArgumentDto arguments);

        GuildEntity[] ReadPending();
        GuildEntity ReadForStudent(int studentId);

        // TODO: extract methods below to GuildMemberRepository
        Boolean IsStudentHaveRequest(int studentId);

        GuildMemberEntity AddMember(GuildEntity guild, Student student, GuildMemberType memberType);
        GuildMemberEntity UpdateMember(GuildMemberEntity member);
        void RemoveMember(int guildId, int userId);

        GuildPinnedProjectEntity PinProject(int guildId, string owner, string projectName);
        void UnpinProject(int pinnedProjectId);
    }
}