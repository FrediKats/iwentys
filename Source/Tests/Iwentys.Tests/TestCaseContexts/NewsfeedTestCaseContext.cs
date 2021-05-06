using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
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
            _context.UnitOfWork.GetRepository<Subject>().Insert(subject);
            _context.UnitOfWork.CommitAsync().Wait();

            return new SubjectProfileDto(subject);
        }
    }
}