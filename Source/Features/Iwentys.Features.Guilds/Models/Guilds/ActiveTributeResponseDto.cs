using System;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record ActiveTributeResponseDto(
        long ProjectId,
        TributeState State,
        string ProjectName,
        DateTime CreationTime)
    {
        public ActiveTributeResponseDto(TributeEntity tribute)
            : this(tribute.ProjectId, tribute.State, tribute.ProjectEntity.Name, tribute.CreationTimeUtc)
        {
        }
    }
}