using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Gamification
{
    [TestFixture]
    public class StudyLeaderboardServiceTest
    {
        [Test]
        public async Task GetStudentActivity_Ok()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            var group = testCase.StudyTestCaseContext.WithStudyGroup();
            Subject subject = testCase.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCase.StudyTestCaseContext.WithGroupSubject(@group, subject);
            Student student = testCase.StudyTestCaseContext.WithNewStudentAsStudent(@group);
            const int pointCount = 10;

            //TODO: refactor
            IGenericRepository<SubjectActivity> repository = testCase.UnitOfWork.GetRepository<SubjectActivity>();
            var activity = new SubjectActivity {GroupSubject = groupSubject, StudentId = student.Id, Points = pointCount };
            repository.Insert(activity);
            await testCase.UnitOfWork.CommitAsync();
            List<StudyLeaderboardRowDto> studyLeaderboardRowDtos = testCase.StudyLeaderboard.GetStudentsRatings(StudySearchParametersDto.ForGroup(@group.Id));

            StudyLeaderboardRowDto? studentResult = studyLeaderboardRowDtos.FirstOrDefault(slr => slr.Student.Id == student.Id);
            Assert.IsNotNull(studentResult);
            Assert.AreEqual(pointCount, studentResult.Activity);
        }
    }
}