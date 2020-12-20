using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Gamification.Entities
{
    public class UserInterestTagEntity
    {
        public int InterestTagId { get; set; }
        public virtual InterestTagEntity InterestTag { get; set; }

        public int StudentId { get; set; }
        public virtual StudentEntity Student { get; set; }
    }
}