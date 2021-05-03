using Iwentys.Common.Tools;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Study.Models
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