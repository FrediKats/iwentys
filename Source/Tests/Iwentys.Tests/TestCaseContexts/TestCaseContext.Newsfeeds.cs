using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithSubject(out SubjectProfileDto subjectProfile)
        {
            var subject = new Subject
            {
                Name = RandomProvider.Faker.Name.JobArea()
            };
            UnitOfWork.GetRepository<Subject>().InsertAsync(subject).Wait();
            UnitOfWork.CommitAsync().Wait();

            subjectProfile = new SubjectProfileDto(subject);
            return this;
        }

        //TODO: return smth
        public TestCaseContext WithSubjectNews(SubjectProfileDto subjectProfile, AuthorizedUser creator)
        {
            var createViewModel = new NewsfeedCreateViewModel()
            {
                Title = RandomProvider.Faker.Lorem.Word(),
                Content = RandomProvider.Faker.Lorem.Sentence()
            };

            NewsfeedService.CreateSubjectNewsfeed(createViewModel, creator, subjectProfile.Id).Wait();
            
            return this;
        }
    }
}