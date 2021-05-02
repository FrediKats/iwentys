using System;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.Study;

namespace Iwentys.Domain.Extended
{
    public class SubjectNewsfeed
    {
        public int SubjectId { get; init; }
        public virtual Subject Subject { get; init; }

        public int NewsfeedId { get; init; }
        public virtual Newsfeed Newsfeed { get; init; }

        public static SubjectNewsfeed CreateAsSystemAdmin(NewsfeedCreateViewModel createViewModel, SystemAdminUser admin, Subject subject)
        {
            return Create(createViewModel, subject, admin.User.Id);
        }

        public static SubjectNewsfeed CreateAsGroupAdmin(NewsfeedCreateViewModel createViewModel, GroupAdminUser groupAdmin, Subject subject)
        {
            return Create(createViewModel, subject, groupAdmin.Student.Id);
        }

        private static SubjectNewsfeed Create(NewsfeedCreateViewModel createViewModel, Subject subject, int authorId)
        {
            var newsfeed = new Newsfeed
            {
                Title = createViewModel.Title,
                Content = createViewModel.Content,
                CreationTimeUtc = DateTime.UtcNow,
                AuthorId = authorId
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