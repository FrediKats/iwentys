using System.Linq;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.DomainModel
{
    public class GuildMentorUser
    {
        public GuildMentorUser(Student student, GuildEntity guild)
        {
            Student = student;
            Guild = guild;
        }

        public Student Student { get; }
        public GuildEntity Guild { get; }
    }

    public static class GuildMentorUserExtensions
    {
        public static GuildMentorUser EnsureIsMentor(this Student student, IGuildRepository guildRepository, int guildId)
        {
            GuildEntity guild = guildRepository.Get(guildId);
            GuildMemberEntity membership = guild.Members.First(m => m.MemberId == student.Id);
            if (!membership.MemberType.IsEditor())
                throw InnerLogicException.NotEnoughPermission(student.Id);

            return new GuildMentorUser(student, guild);
        }
    }
}