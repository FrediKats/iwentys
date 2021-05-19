using System;
using System.Collections.Generic;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Raids.Models;

namespace Iwentys.Domain.Raids
{
    public class Raid
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public RaidType RaidType { get; set; }

        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public virtual ICollection<RaidVisitor> Visitors { get; set; }
        public virtual ICollection<RaidPartySearchRequest> PartySearchRequests { get; set; }

        public static Raid CreateCommon(IwentysUser admin, RaidCreateArguments arguments)
        {
            SystemAdminUser systemAdminUser = admin.EnsureIsAdmin();

            return new Raid
            {
                Title = arguments.Title,
                Description = arguments.Description,
                CreationTimeUtc = DateTime.UtcNow,
                StartTimeUtc = arguments.StartTimeUtc,
                EndTimeUtc = arguments.EndTimeUtc,
                RaidType = RaidType.PublicLecture,
                AuthorId = systemAdminUser.User.Id
            };
        }

        public RaidVisitor RegisterVisitor(IwentysUser visitor)
        {
            if (RaidType == RaidType.PublicLecture) return RaidVisitor.CreateForLecture(Id, visitor.Id);

            return RaidVisitor.CreateRequest(Id, visitor.Id);
        }

        public RaidPartySearchRequest CreatePartySearchRequest(RaidVisitor visitor, RaidPartySearchRequestArguments arguments)
        {
            return RaidPartySearchRequest.Create(this, visitor, arguments);
        }
    }
}