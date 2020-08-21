using System;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Transferable.Guilds
{
    public class ActiveTributeDto
    {
        public long ProjectId { get; set; }
        public TributeState State { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreationTime { get; set; }

        public static ActiveTributeDto Create(Tribute tribute)
        {
            return new ActiveTributeDto
            {
                ProjectId = tribute.ProjectId,
                State = tribute.State,
                ProjectName = tribute.Project.Name,
                CreationTime = tribute.CreationTime
            };
        }
    }
}