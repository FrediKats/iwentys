using System.Threading.Tasks;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildMemberRepository
    {
        bool IsStudentHaveRequest(int studentId);
        Task<GuildMemberEntity> AddMemberAsync(GuildEntity guild, StudentEntity student, GuildMemberType memberType);
        Task<GuildMemberEntity> UpdateMemberAsync(GuildMemberEntity member);
        void RemoveMemberAsync(int guildId, int userId);
    }
}