using System;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Models
{
    public record StudentInfoDto : IResultFormat
    {
        public StudentInfoDto(StudentEntity student) : this(
            student.Id,
            student.FirstName,
            student.MiddleName,
            student.SecondName,
            student.Role,
            student.Type,
            student.GithubUsername,
            student.CreationTime,
            student.LastOnlineTime,
            student.BarsPoints,
            student.AvatarUrl)
        {
        }

        public StudentInfoDto(int id, string firstName, string middleName, string secondName, UserType role,
            StudentType type, string githubUsername, DateTime creationTime, DateTime lastOnlineTime, int barsPoints,
            string avatarUrl)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            SecondName = secondName;
            Role = role;
            Type = type;
            GithubUsername = githubUsername;
            CreationTime = creationTime;
            LastOnlineTime = lastOnlineTime;
            BarsPoints = barsPoints;
            AvatarUrl = avatarUrl;
        }

        public StudentInfoDto()
        {
        }

        public int Id { get; init; }
        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }
        public UserType Role { get; init; }
        public StudentType Type { get; init; }
        public string GithubUsername { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime LastOnlineTime { get; init; }
        public int BarsPoints { get; init; }
        public string AvatarUrl { get; init; }

        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public string Format()
        {
            return $"{Id} {GetFullName()}";
        }

        public string GetFullName()
        {
            return $"{FirstName} {SecondName}";
        }
    }
}