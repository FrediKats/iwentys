using System;
using Iwentys.Features.Students.Domain;
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

        public static KarmaUpVote Create(AuthorizedUser author, Student target)
        {
            return new KarmaUpVote
            {
                AuthorId = author.Id,
                TargetId = target.Id,
                CreationTimeUtc = DateTime.UtcNow
            };
        }
    }
}