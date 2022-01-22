using System.Collections.Generic;
using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.WebService.Application;

public static class EntityManagerApiDtoMapper
{
    private static readonly IMapper Mapper;

    static EntityManagerApiDtoMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Student, StudentInfoDto>();
            cfg.CreateMap<IwentysUserInfoDto, IwentysUser>();
        });

        Mapper = new Mapper(config);
    }

    public static IwentysUserInfoDto Map(IwentysUser user)
    {
        return Mapper.Map<IwentysUserInfoDto>(user);
    }

    public static IwentysUser Map(IwentysUserInfoDto dto)
    {
        return Mapper.Map<IwentysUser>(dto);
    }

    public static StudentInfoDto Map(Student student)
    {
        return Mapper.Map<StudentInfoDto>(student);
    }
}