using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class SubjectNewsfeedEntity
    {
        public int SubjectId { get; set; }
        public SubjectEntity Subject { get; set; }

        public int NewsfeedId { get; set; }
        public NewsfeedEntity Newsfeed { get; set; }
    }
}