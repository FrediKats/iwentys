using System;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;

namespace Iwentys.Domain.Newsfeeds;

public class SubjectNewsfeed
{
    public int SubjectId { get; init; }

    public int NewsfeedId { get; init; }
    public virtual Newsfeed Newsfeed { get; init; }

    public static SubjectNewsfeed CreateAsSystemAdmin(NewsfeedCreateViewModel createViewModel, SystemAdminUser admin, int subjectId)
    {
        return Create(createViewModel, subjectId, admin.User.Id);
    }

    public static SubjectNewsfeed CreateAsGroupAdmin(NewsfeedCreateViewModel createViewModel, Student student, int subjectId)
    {
        return Create(createViewModel, subjectId, student.Id);
    }

    private static SubjectNewsfeed Create(NewsfeedCreateViewModel createViewModel, int subjectId, int authorId)
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
            SubjectId = subjectId
        };

        return subjectNewsfeed;
    }
}