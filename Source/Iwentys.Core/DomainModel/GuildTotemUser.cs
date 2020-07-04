using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;

namespace Iwentys.Core.DomainModel
{
    public class GuildTotemUser
    {
        public GuildTotemUser(Student student, Guild guild)
        {
            Student = student;
            Guild = guild;
        }

        public Student Student { get; }
        public Guild Guild { get; }
    }

    public static class GuildTotemUserExtensions
    {
        public static GuildTotemUser EnsureIsTotem(this AuthorizedUser user, IGuildRepository guildRepository, int guildId)
        {
            Guild guild = guildRepository.Get(guildId);
            if (guild.TotemId != user.Id)
                throw InnerLogicException.NotEnoughPermission(user.Id);

            return new GuildTotemUser(user.Profile, guild);
        }
    }
}