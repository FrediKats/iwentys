using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Repositories;
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
        public static async Task<GuildMentorUser> EnsureIsMentor(this StudentEntity student, IGuildRepository guildRepository, int guildId)
        {
            GuildEntity guild = await guildRepository.GetAsync(guildId);
            GuildMemberEntity membership = guild.Members.First(m => m.MemberId == student.Id);
            if (!membership.MemberType.IsEditor())
                throw InnerLogicException.NotEnoughPermission(student.Id);

            return new GuildMentorUser(student, guild);
        }
    }
}