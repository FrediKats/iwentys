using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Enums;

namespace Iwentys.Features.Study.Models.Students
{
    public class StudentInfoDto : IwentysUserInfoDto, IResultFormat
    {
        public StudentInfoDto(Student student) : base(student)
        {
            Type = student.Type;
            GroupId = student.GroupId;
        }

        public StudentInfoDto()
        {
        }

        public StudentType Type { get; init; }
        public int? GroupId { get; set; }

        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public string Format()
        {
            return $"{Id} {GetFullName()}";
        }
    }
}