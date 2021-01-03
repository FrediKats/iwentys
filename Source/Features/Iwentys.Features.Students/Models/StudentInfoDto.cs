using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Models
{
    public class StudentInfoDto : IwentysUserInfoDto, IResultFormat
    {
        public StudentInfoDto(Student student) : base(student)
        {
            Role = student.Role;
            Type = student.Type;
        }

        public StudentInfoDto()
        {
        }

        public StudentRole Role { get; init; }
        public StudentType Type { get; init; }

        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public string Format()
        {
            return $"{Id} {GetFullName()}";
        }
    }
}