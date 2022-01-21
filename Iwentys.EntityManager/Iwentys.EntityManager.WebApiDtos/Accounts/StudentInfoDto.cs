using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.WebApiDtos;

public class StudentInfoDto : IwentysUserInfoDto
{
    public StudentType Type { get; init; }
    public int? GroupId { get; set; }

    //public string SocialStatus { get; set; }
    //public string AdditionalLink { get; set; }
}