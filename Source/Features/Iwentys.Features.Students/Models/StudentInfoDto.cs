using System;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Models
{
    public record StudentInfoDto(
        int Id,
        string FirstName,
        string MiddleName,
        string SecondName,
        UserType Role,
        StudentType Type,
        string GithubUsername,
        DateTime CreationTime,
        DateTime LastOnlineTime,
        int BarsPoints,
        string AvatarUrl) : IResultFormat
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