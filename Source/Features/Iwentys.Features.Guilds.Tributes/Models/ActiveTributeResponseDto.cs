using System;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Guilds.Tributes.Enums;

namespace Iwentys.Features.Guilds.Tributes.Models
{
    public record ActiveTributeResponseDto
    {
        public ActiveTributeResponseDto(TributeEntity tribute)
            : this(tribute.ProjectId, tribute.State, tribute.Project.Name, tribute.CreationTimeUtc)
        {
        }

        public ActiveTributeResponseDto(long projectId, TributeState state, string projectName, DateTime creationTime)
        {
            ProjectId = projectId;
            State = state;
            ProjectName = projectName;
            CreationTime = creationTime;
        }

        public ActiveTributeResponseDto()
        {
        }
        
        public long ProjectId { get; init; }
        public TributeState State { get; init; }
        public string ProjectName { get; init; }
        public DateTime CreationTime { get; init; }
    }
}