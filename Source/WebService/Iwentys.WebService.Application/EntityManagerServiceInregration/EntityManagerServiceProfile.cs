using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.WebService.Application;

public class EntityManagerServiceProfile : Profile
{
    public EntityManagerServiceProfile()
    {
        CreateMap<Student, StudentInfoDto>();
        CreateMap<IwentysUserInfoDto, IwentysUser>();
    }
}