using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Gamification.Entities
{
    public class StudentInterestTag
    {
        public int InterestTagId { get; set; }
        public virtual InterestTag InterestTag { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}