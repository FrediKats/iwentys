using System.Threading.Tasks;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

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