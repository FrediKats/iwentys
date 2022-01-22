using AutoMapper;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;

namespace Iwentys.EntityManager.WebApi;

public class EntityManagerMappingProfile : Profile
{
    public EntityManagerMappingProfile()
    {
        CreateMap<UniversitySystemUser, UniversitySystemUserInfoDto>();
        CreateMap<IwentysUser, IwentysUserInfoDto>();
        CreateMap<Student, StudentInfoDto>();

        CreateMap<Subject, SubjectProfileDto>();
        CreateMap<StudyGroup, StudyGroupProfileResponsePreviewDto>();
        CreateMap<StudyGroup, StudyGroupProfileResponseDto>();
        CreateMap<GroupSubject, GroupSubjectInfoDto>();

        CreateMapForMentors();
    }

    public void CreateMapForMentors()
    {
        CreateMap<IGrouping<int, GroupSubject>, SubjectTeachersDto>()
            .ForMember(s => s.Name,
                map =>
                    map.MapFrom(g => g.First().Subject.Title))
            .ForMember(s => s.SubjectId,
                map =>
                    map.MapFrom(g => g.Key))
            .ForMember(s => s.GroupTeachers,
                map =>
                    map.MapFrom(g => g.Select(x => x)));

        CreateMap<GroupSubject, GroupTeachersDto>()
            .ForMember(gm => gm.GroupName,
                map =>
                    map.MapFrom(gs => gs.StudyGroup.GroupName));
        CreateMap<IwentysUser, TeacherDto>();

        CreateMap<GroupSubjectTeacher, TeacherDto>()
            .ForMember(gs => gs.Id,
                map =>
                    map.MapFrom(gsm => gsm.TeacherId))
            .ForMember(gs => gs.FirstName,
                map =>
                    map.MapFrom(gsm => gsm.Teacher.FirstName))
            .ForMember(gs => gs.MiddleName,
                map =>
                    map.MapFrom(gsm => gsm.Teacher.MiddleName))
            .ForMember(gs => gs.SecondName,
                map =>
                    map.MapFrom(gsm => gsm.Teacher.SecondName));
    }
}