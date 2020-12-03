using System;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.ViewModels.Guilds
{
    public class ActiveTributeResponse
    {
        public long ProjectId { get; set; }
        public TributeState State { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreationTime { get; set; }

        public static ActiveTributeResponse Create(TributeEntity tribute)
        {
            return new ActiveTributeResponse
            {
                ProjectId = tribute.ProjectId,
                State = tribute.State,
                ProjectName = tribute.ProjectEntity.Name,
                CreationTime = tribute.CreationTime
            };
        }
    }
}