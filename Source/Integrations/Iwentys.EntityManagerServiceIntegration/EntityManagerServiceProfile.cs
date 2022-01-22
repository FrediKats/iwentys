using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.EntityManagerServiceIntegration;

public class EntityManagerServiceProfile : Profile
{
    public EntityManagerServiceProfile()
    {
        CreateMap<Student, StudentInfoDto>();
        CreateMap<StudentInfoDto, Student>();
        CreateMap<IwentysUserInfoDto, IwentysUser>();
    }
}