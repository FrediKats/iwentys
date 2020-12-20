using System;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class SubjectNewsfeedEntity
    {
        public int SubjectId { get; set; }
        public virtual SubjectEntity Subject { get; set; }

        public int NewsfeedId { get; set; }
        public virtual NewsfeedEntity Newsfeed { get; set; }

        public static SubjectNewsfeedEntity Create(NewsfeedCreateViewModel createViewModel, StudentEntity student, SubjectEntity subject)
        {
            var newsfeed = new NewsfeedEntity
            {
                Title = createViewModel.Title,
                Content = createViewModel.Content,
                CreationTimeUtc = DateTime.UtcNow,
                AuthorId = student.Id
            };

            var subjectNewsfeed = new SubjectNewsfeedEntity
            {
                Newsfeed = newsfeed,
                SubjectId = subject.Id
            };

            return subjectNewsfeed;
        }
    }
}