using System;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Guilds
{
    public class ActiveTributeDto
    {
        public long ProjectId { get; set; }
        public TributeState State { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreationTime { get; set; }

        public static ActiveTributeDto Create(TributeEntity tribute)
        {
            return new ActiveTributeDto
            {
                ProjectId = tribute.ProjectId,
                State = tribute.State,
                ProjectName = tribute.ProjectEntity.Name,
                CreationTime = tribute.CreationTime
            };
        }
    }
}