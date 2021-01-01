using System;
using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Raids.Enums;
using Iwentys.Features.Raids.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Iwentys.Features.Raids.Entities
{
    public class Raid
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RaidType RaidType { get; set; }

        public int AuthorId { get; set; }
        public virtual Student Author { get; set; }

        public virtual ICollection<RaidVisitor> Visitors { get; set; }
        public virtual ICollection<RaidInterestTag> InterestTags { get; set; }

        public static Raid CreateCommon(SystemAdminUser admin, RaidCreateArguments arguments)
        {
            return new Raid
            {
                Title = arguments.Title,
                Description = arguments.Description,
                CreationTime = DateTime.UtcNow,
                StartTime = arguments.StartTimeUtc,
                EndTime = arguments.EndTimeUtc,
                RaidType = RaidType.PublicLecture,
                AuthorId = admin.Student.Id
            };
        }

        public RaidVisitor RegisterVisitor(Student visitor)
        {
            if (RaidType == RaidType.PublicLecture)
            {
                return RaidVisitor.CreateForLecture(Id, visitor.Id);
            }

            return RaidVisitor.CreateRequest(Id, visitor.Id);
        }
    }
}