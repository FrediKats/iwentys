using System.Linq;

using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel
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
        public static GuildMentorUser EnsureIsMentor(this StudentEntity student, GuildRepository guildRepository, int guildId)
        {
            GuildEntity guild = guildRepository.Get(guildId);
            GuildMemberEntity membership = guild.Members.First(m => m.MemberId == student.Id);
            if (!membership.MemberType.IsEditor())
                throw InnerLogicException.NotEnoughPermission(student.Id);

            return new GuildMentorUser(student, guild);
        }
    }
}