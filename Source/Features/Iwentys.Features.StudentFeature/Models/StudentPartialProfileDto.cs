using System;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Enums;

namespace Iwentys.Features.StudentFeature.Models
{
    public class StudentPartialProfileDto : IResultFormat
    {
        public StudentPartialProfileDto()
        {
        }

        public StudentPartialProfileDto(StudentEntity student) : this()
        {
            Id = student.Id;
            FirstName = student.FirstName;
            MiddleName = student.MiddleName;
            SecondName = student.SecondName;
            Role = student.Role;
            Type = student.Type;
            GithubUsername = student.GithubUsername;
            CreationTime = student.CreationTime;
            LastOnlineTime = student.LastOnlineTime;
            BarsPoints = student.BarsPoints;
            AvatarUrl = student.AvatarUrl;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public StudentType Type { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }
        public string AvatarUrl { get; set; }

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