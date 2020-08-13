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
        public GuildMentorUser(Student student, Guild guild)
        {
            Student = student;
            Guild = guild;
        }

        public Student Student { get; }
        public Guild Guild { get; }
    }

    public static class GuildMentorUserExtensions
    {
        public static GuildMentorUser EnsureIsMentor(this AuthorizedUser user, IGuildRepository guildRepository, int guildId)
        {
            Guild guild = guildRepository.Get(guildId);
            GuildMember membership = guild.Members.First(m => m.MemberId == user.Id);
            if (!membership.MemberType.IsEditor())
                throw InnerLogicException.NotEnoughPermission(user.Id);

            return new GuildMentorUser(user.Profile, guild);
        }
    }
}