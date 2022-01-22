using System.Linq;
using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.AccountManagement;

public class AccountManagementMappingProfile : Profile
{
    public AccountManagementMappingProfile()
    {
        CreateMapForUsers();
        CreateMapForMentors();
    }

    public void CreateMapForUsers()
    {
        CreateMap<IwentysUser, IwentysUserInfoDto>();
        CreateMap<Student, StudentInfoDto>();
    }

    public void CreateMapForMentors()
    {
        CreateMap<IGrouping<int, GroupSubject>, SubjectMentorsDto>()
            .ForMember(s => s.Name,
                map =>
                    map.MapFrom(g => g.First().Subject.Title))
            .ForMember(s => s.Id,
                map =>
                    map.MapFrom(g => g.Key))
            .ForMember(s => s.Groups,
                map =>
                    map.MapFrom(g => g.Select(x => x)));

        CreateMap<GroupSubject, GroupMentorsDto>()
            .ForMember(gm => gm.GroupName,
                map =>
                    map.MapFrom(gs => gs.StudyGroup.GroupName));
        CreateMap<IwentysUser, MentorDto>();
    }
}