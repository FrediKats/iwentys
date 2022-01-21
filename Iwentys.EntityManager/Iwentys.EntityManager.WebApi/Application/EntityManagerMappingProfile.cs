using AutoMapper;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.Domain.Accounts;
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
        CreateMap<GroupSubject, GroupSubjectInfoDto>();

        CreateMap<StudyCourse, StudyCourseInfoDto>();


    }
}