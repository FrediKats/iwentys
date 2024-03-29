﻿using AutoMapper;
using Iwentys.Domain.GithubIntegration;

namespace Iwentys.WebService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMapForProject();
    }

    public void CreateMapForProject()
    {
        CreateMap<GithubProject, GithubRepositoryInfoDto>();
    }
}