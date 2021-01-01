using Iwentys.Features.Raids.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Raids.Entities
{
    public class RaidVisitor
    {
        public int RaidId { get; set; }
        public virtual Raid Raid { get; set; }

        public int VisitorId { get; set; }
        public virtual Student Visitor { get; set; }

        public RaidVisitorRole Role { get; set; }
    }
}