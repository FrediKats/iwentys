using System.Linq;
using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;

namespace Iwentys.AccountManagement
{
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

            CreateMap<GroupSubjectMentor, MentorDto>()
                .ForMember(gs => gs.Id,
                    map =>
                        map.MapFrom(gsm => gsm.UserId))
                .ForMember(gs => gs.FirstName,
                    map =>
                        map.MapFrom(gsm => gsm.User.FirstName))
                .ForMember(gs => gs.MiddleName,
                    map =>
                        map.MapFrom(gsm => gsm.User.MiddleName))
                .ForMember(gs => gs.SecondName,
                    map =>
                        map.MapFrom(gsm => gsm.User.SecondName));
        }
    }
}