using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Enums;

namespace Iwentys.Domain
{
    public class RaidVisitor
    {
        public int RaidId { get; set; }
        public virtual Raid Raid { get; set; }

        public int VisitorId { get; set; }
        public virtual IwentysUser Visitor { get; set; }

        public RaidVisitorRole Role { get; set; }

        public static RaidVisitor CreateForLecture(int raidId, int visitorId)
        {
            return new RaidVisitor
            {
                RaidId = raidId,
                VisitorId = visitorId,
                Role = RaidVisitorRole.Approved
            };
        }

        public static RaidVisitor CreateRequest(int raidId, int visitorId)
        {
            return new RaidVisitor
            {
                RaidId = raidId,
                VisitorId = visitorId,
                Role = RaidVisitorRole.Pending
            };
        }

        public void Approve()
        {
            if (Role != RaidVisitorRole.Pending)
                throw InnerLogicException.RaidExceptions.RequestIsNotActual(RaidId, VisitorId);

            Role = RaidVisitorRole.Approved;
        }
    }
}