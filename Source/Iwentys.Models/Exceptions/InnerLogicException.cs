using System;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Models.Exceptions
{
    public class InnerLogicException : IwentysException
    {
        public InnerLogicException(string message) : base(message)
        {
        }

        public static InnerLogicException NotEnoughPermission(int userId)
        {
            return new InnerLogicException($"Not enough user permission for user {userId}");
        }

        public static InnerLogicException NotSupportedEnumValue<T>(T value) where T : Enum
        {
            return new InnerLogicException($"Unsupported [{value.GetType()}] type: {value}");
        }

        public static InnerLogicException NotEnoughBarsPoints()
        {
            return new InnerLogicException("Student don't have enough points for this operation.");
        }

        public static class Guild
        {
            public static InnerLogicException IsNotGuildMember(int studentId, int? guildId)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.GuildExceptions.IsNotGuildMember, studentId, guildId));
            }

            public static InnerLogicException CreatorCannotLeave(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.GuildExceptions.CreatorCannotLeave, studentId, guildId));
            }

            public static InnerLogicException RequestWasNotFound(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.GuildExceptions.RequestWasNotFound, studentId, guildId));
            }

            public static InnerLogicException StudentCannotBeBlocked(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.GuildExceptions.StudentCannotBeBlocked, studentId, guildId));
            }

            public static InnerLogicException IsNotGuildEditor(int studentId) => new InnerLogicException($"Student is not guild editor. Id: [{studentId}]");
        }

        public static class TributeEx
        {
            public static InnerLogicException ProjectAlreadyUsed(int projectId)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.TributeExceptions.ProjectAlreadyUsed, projectId));
            }

            public static InnerLogicException UserAlreadyHaveTribute(int userId)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.TributeExceptions.UserAlreadyHaveTribute, userId));
            }

            public static InnerLogicException IsNotActive(Tribute tribute)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.TributeExceptions.IsNotActive, tribute.ProjectId, tribute.State));
            }
        }

        public static class StudentEx
        {
            public static InnerLogicException GithubAlreadyUser(string githubUsername)
            {
                return new InnerLogicException(string.Format(ExceptionMessages.StudentEx.GithubAlreadyUser, githubUsername));
            }
        }
    }
}