using System;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class SubjectNewsfeed
    {
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int NewsfeedId { get; set; }
        public virtual Newsfeed Newsfeed { get; set; }

        public static SubjectNewsfeed Create(NewsfeedCreateViewModel createViewModel, Student student, Subject subject)
        {
            var newsfeed = new Newsfeed
            {
                Title = createViewModel.Title,
                Content = createViewModel.Content,
                CreationTimeUtc = DateTime.UtcNow,
                AuthorId = student.Id
            };

            var subjectNewsfeed = new SubjectNewsfeed
            {
                Newsfeed = newsfeed,
                SubjectId = subject.Id
            };

            return subjectNewsfeed;
        }
    }
}