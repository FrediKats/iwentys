using System;
using Iwentys.Features.Raids.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Raids.Entities
{
    public class RaidPartySearchRequest
    {
        public int RaidId { get; set; }
        public virtual Raid Raid { get; set; }

        public int AuthorId { get; set; }
        public virtual Student Author { get; set; }

        public string Description { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public static RaidPartySearchRequest Create(Raid raid, RaidVisitor visitor, RaidPartySearchRequestArguments arguments)
        {
            return new RaidPartySearchRequest
            {
                RaidId = raid.Id,
                AuthorId = visitor.VisitorId,
                Description = arguments.Description,
                CreationTimeUtc = DateTime.UtcNow
            };
        }
    }
}