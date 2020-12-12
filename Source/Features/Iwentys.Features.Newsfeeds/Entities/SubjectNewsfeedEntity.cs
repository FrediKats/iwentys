using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class SubjectNewsfeedEntity
    {
        public int SubjectId { get; set; }
        public virtual SubjectEntity Subject { get; set; }

        public int NewsfeedId { get; set; }
        public virtual NewsfeedEntity Newsfeed { get; set; }
    }
}