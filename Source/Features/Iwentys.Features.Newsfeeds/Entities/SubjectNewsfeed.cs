using System;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class SubjectNewsfeed
    {
        public int SubjectId { get; init; }
        public virtual Subject Subject { get; init; }

        public int NewsfeedId { get; init; }
        public virtual Newsfeed Newsfeed { get; init; }

        public static SubjectNewsfeed Create(NewsfeedCreateViewModel createViewModel, IwentysUser author, Subject subject, StudyGroup studyGroup)
        {
            //TODO: remove possible NRE
            if (!author.IsAdmin && author.Id != studyGroup?.GroupAdminId)
                throw InnerLogicException.NotEnoughPermissionFor(author.Id);

            var newsfeed = new Newsfeed
            {
                Title = createViewModel.Title,
                Content = createViewModel.Content,
                CreationTimeUtc = DateTime.UtcNow,
                AuthorId = author.Id
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