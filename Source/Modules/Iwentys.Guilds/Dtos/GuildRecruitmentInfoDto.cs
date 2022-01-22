using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;

namespace Iwentys.Guilds;

public class GuildRecruitmentInfoDto
{
    public int Id { get; init; }

    public IwentysUserInfoDto Author { get; set; }

    public string Description { get; init; }
    public bool IsActive { get; set; }
    public virtual List<IwentysUserInfoDto> RecruitmentMembers { get; init; }

    public static Expression<Func<GuildRecruitment, GuildRecruitmentInfoDto>> FromEntity =>
        entity => new GuildRecruitmentInfoDto
        {
            Id = entity.Id,
            Author = EntityManagerApiDtoMapper.Map(entity.Author),
            Description = entity.Description,
            IsActive = entity.IsActive,
            RecruitmentMembers = entity.RecruitmentMembers.Select(m => EntityManagerApiDtoMapper.Map(m.Member)).ToList()
        };
}