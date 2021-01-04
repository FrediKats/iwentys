using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public class NewsfeedTestCaseContext
    {
        private readonly TestCaseContext _context;

        public NewsfeedTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public SubjectProfileDto WithSubject()
        {
            var subject = new Subject
            {
                Name = RandomProvider.Faker.Name.JobArea()
            };
            _context.UnitOfWork.GetRepository<Subject>().InsertAsync(subject).Wait();
            _context.UnitOfWork.CommitAsync().Wait();

            return new SubjectProfileDto(subject);
        }

        //TODO: return smth
        public void WithSubjectNews(SubjectProfileDto subjectProfile, AuthorizedUser creator)
        {
            var createViewModel = new NewsfeedCreateViewModel()
            {
                Title = RandomProvider.Faker.Lorem.Word(),
                Content = RandomProvider.Faker.Lorem.Sentence()
            };

            _context.NewsfeedService.CreateSubjectNewsfeed(createViewModel, creator, subjectProfile.Id).Wait();
        }
    }
}