﻿using System;
using System.Globalization;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.ExceptionMessages;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Models.Exceptions
{
    public class InnerLogicException : IwentysException
    {
        public InnerLogicException(string message) : base(message)
        {
        }

        public InnerLogicException()
        {
        }

        public InnerLogicException(string message, Exception innerException) : base(message, innerException)
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
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptions.IsNotGuildMember, studentId, guildId));
            }

            public static InnerLogicException CreatorCannotLeave(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptions.CreatorCannotLeave, studentId, guildId));
            }

            public static InnerLogicException RequestWasNotFound(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptions.RequestWasNotFound, studentId, guildId));
            }

            public static InnerLogicException StudentCannotBeBlocked(int studentId, int guildId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, GuildExceptions.StudentCannotBeBlocked, studentId, guildId));
            }

            public static InnerLogicException IsNotGuildEditor(int studentId)
            {
                return new InnerLogicException($"Student is not guild editor. Id: [{studentId}]");
            }
        }

        public static class TributeEx
        {
            public static InnerLogicException ProjectAlreadyUsed(long projectId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptions.ProjectAlreadyUsed, projectId));
            }

            public static InnerLogicException UserAlreadyHaveTribute(int userId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptions.UserAlreadyHaveTribute, userId));
            }

            public static InnerLogicException IsNotActive(TributeEntity tribute)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptions.IsNotActive, tribute.ProjectId, tribute.State));
            }

            public static InnerLogicException TributeCanBeSendFromStudentAccount(StudentEntity student, CreateProjectDto createProject)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptions.TributeCanBeSendFromStudentAccount, student.Id, createProject.Owner));
            }
        }

        public static class StudentEx
        {
            public static InnerLogicException GithubAlreadyUser(string githubUsername)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, ExceptionMessages.StudentEx.GithubAlreadyUser, githubUsername));
            }
        }
    }
}