using System;

namespace Iwentys.Domain.Guilds;

public record ActiveTributeResponseDto
{
    public ActiveTributeResponseDto(Tribute tribute, DateTime lastUpdateTimeUtc)
        : this(tribute.ProjectId, tribute.State, tribute.Project.Name, tribute.CreationTimeUtc, lastUpdateTimeUtc)
    {
    }

    public ActiveTributeResponseDto(long projectId, TributeState state, string projectName, DateTime creationTime, DateTime lastUpdateTimeUtc) : this()
    {
        ProjectId = projectId;
        State = state;
        ProjectName = projectName;
        CreationTime = creationTime;
        LastUpdateTimeUtc = lastUpdateTimeUtc;
    }

    public ActiveTributeResponseDto()
    {
    }

    public long ProjectId { get; init; }
    public TributeState State { get; init; }
    public string ProjectName { get; init; }
    public DateTime CreationTime { get; init; }
    public DateTime LastUpdateTimeUtc { get; }
}