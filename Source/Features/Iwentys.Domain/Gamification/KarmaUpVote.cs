using System;

namespace Iwentys.Domain.Gamification
{
    public class KarmaUpVote
    {
        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public int TargetId { get; set; }
        public virtual IwentysUser Target { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public static KarmaUpVote Create(AuthorizedUser author, IwentysUser target)
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