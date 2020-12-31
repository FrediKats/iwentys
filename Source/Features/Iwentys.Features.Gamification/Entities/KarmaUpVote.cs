using System;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Gamification.Entities
{
    public class KarmaUpVote
    {
        public int AuthorId { get; set; }
        public virtual Student Author { get; set; }

        public int TargetId { get; set; }
        public virtual Student Target { get; set; }

        public DateTime CreationTimeUtc { get; set; }
    }
}