using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public class StudyTestCaseContext
    {
        private readonly TestCaseContext _context;

        public StudyTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public StudyGroup WithStudyGroup()
        {
            var studyGroup = new StudyGroup();

            _context.UnitOfWork.GetRepository<StudyGroup>().InsertAsync(studyGroup).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            return studyGroup;
        }

        public AuthorizedUser WithNewStudent(StudyGroup studyGroup)
        {
            int id = RandomProvider.Random.Next(999999);

            var userInfo = new Student
            {
                Id = id,
                GithubUsername = $"{TestCaseContext.Constants.GithubUsername}{id}",
                BarsPoints = 1000
            };

            _context.UnitOfWork.GetRepository<Student>().InsertAsync(userInfo).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            var user = AuthorizedUser.DebugAuth(userInfo.Id);
            return user;
        }
    }
}