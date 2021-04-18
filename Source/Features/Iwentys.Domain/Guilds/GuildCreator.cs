using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Enums;

namespace Iwentys.Domain.Guilds
{
    public class GuildCreator
    {
        public GuildCreator(IwentysUser student, Guild guild, GuildMemberType memberType)
        {
            if (memberType != GuildMemberType.Creator)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            Student = student;
            Guild = guild;
            MemberType = memberType;
        }

        public IwentysUser Student { get; }
        public Guild Guild { get; }
        public GuildMemberType MemberType { get; }
    }

    public static class GuildCreatorExtensions
    {
        public static async Task<GuildCreator> EnsureIsCreator(this IwentysUser student, IGenericRepository<Guild> guildRepository, int guildId)
        {
            Guild guild = await guildRepository.FindByIdAsync(guildId);
            GuildMember membership = guild.Members.First(m => m.MemberId == student.Id);

            return new GuildCreator(student, guild, membership.MemberType);
        }
    }
}