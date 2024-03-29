﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.AccountManagement;
using Iwentys.Domain.Raids;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;

namespace Iwentys.Gamification;

public class RaidProfileDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public RaidType RaidType { get; set; }

    public IwentysUserInfoDto Author { get; set; }

    public ICollection<IwentysUserInfoDto> Visitors { get; set; }
    public ICollection<RaidPartySearchRequestDto> PartySearchRequests { get; set; }

    public static Expression<Func<Raid, RaidProfileDto>> FromEntity =>
        entity => new RaidProfileDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CreationTime = entity.CreationTimeUtc,
            StartTime = entity.StartTimeUtc,
            EndTime = entity.EndTimeUtc,
            RaidType = entity.RaidType,
            Author = EntityManagerApiDtoMapper.Map(entity.Author),
            Visitors = entity.Visitors.Select(s => EntityManagerApiDtoMapper.Map(s.Visitor)).ToList(),
            PartySearchRequests = entity.PartySearchRequests.Select(t => new RaidPartySearchRequestDto(t)).ToList()
        };
}