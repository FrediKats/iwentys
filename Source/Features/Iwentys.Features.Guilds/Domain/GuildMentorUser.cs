using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Domain
{
    public class GuildMentorUser
    {
        public GuildMentorUser(StudentEntity student, GuildEntity guild)
        {
            Student = student;
            Guild = guild;
        }

        public StudentEntity Student { get; }
        public GuildEntity Guild { get; }
    }

    public static class GuildMentorUserExtensions
    {
        public static async Task<GuildMentorUser> EnsureIsMentor(this StudentEntity student, IGenericRepository<GuildEntity> guildRepository, int guildId)
        {
            GuildEntity guild = await guildRepository.FindByIdAsync(guildId);
            GuildMemberEntity membership = guild.Members.First(m => m.MemberId == student.Id);
            if (!membership.MemberType.IsEditor())
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            return new GuildMentorUser(student, guild);
        }
    }
}