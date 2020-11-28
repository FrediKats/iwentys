using Iwentys.Models.Entities.Study;

namespace Iwentys.Models.Entities.Newsfeeds
{
    public class SubjectNewsfeedEntity
    {
        public int SubjectId { get; set; }
        public SubjectEntity Subject { get; set; }

        public int NewsfeedId { get; set; }
        public NewsfeedEntity Newsfeed { get; set; }
    }
}