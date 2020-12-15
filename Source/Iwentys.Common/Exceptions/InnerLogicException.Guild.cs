using System.Globalization;
using Iwentys.Common.ExceptionMessages;

namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class Guild
        {
            public static InnerLogicException ActiveTestExisted(int userId, int guildId)
            {
                return new InnerLogicException($"Test task for {guildId} guild from {userId} user already existed");
            }
            public static InnerLogicException IsNotGuildMember(int studentId, int? guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptionMessages.IsNotGuildMember, studentId, guildId));
            }

            public static InnerLogicException CreatorCannotLeave(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptionMessages.CreatorCannotLeave, studentId, guildId));
            }

            public static InnerLogicException RequestWasNotFound(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptionMessages.RequestWasNotFound, studentId, guildId));
            }

            public static InnerLogicException StudentCannotBeBlocked(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptionMessages.StudentCannotBeBlocked, studentId, guildId));
            }

            public static InnerLogicException IsNotGuildEditor(int studentId)
            {
                return new InnerLogicException($"Student is not guild editor. Id: [{studentId}]");
            }

            public static EntityNotFoundException PinnedRepoWasNotFound(int pinnedRepoId)
            {
                return new EntityNotFoundException(string.Format(GuildExceptionMessages.PinnedRepoWasNotFound, pinnedRepoId));
            }
        }
    }
}
