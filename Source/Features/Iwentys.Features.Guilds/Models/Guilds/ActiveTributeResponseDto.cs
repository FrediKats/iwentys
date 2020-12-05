using System;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class ActiveTributeResponseDto
    {
        public long ProjectId { get; set; }
        public TributeState State { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreationTime { get; set; }

        public static ActiveTributeResponseDto Create(TributeEntity tribute)
        {
            return new ActiveTributeResponseDto
            {
                ProjectId = tribute.ProjectId,
                State = tribute.State,
                ProjectName = tribute.ProjectEntity.Name,
                CreationTime = tribute.CreationTime
            };
        }
    }
}