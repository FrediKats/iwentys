using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Common;
using Iwentys.Domain.Study;
using Iwentys.Gamification;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Gamification
{
    [TestFixture]
    public class StudyLeaderboardServiceTest
    {
        [Test]
        public void GetStudentActivity_Ok()
        {
            const int groupId = 10;
            TestCaseContext testCase = TestCaseContext.Case();

            var studyGroup = new StudyGroup
            {
                Id = groupId,
                GroupName = new Faker().Lorem.Word()
            };

            var subject = new Subject
            {
                Title = new Faker().Lorem.Word()
            };

            var groupSubject = new GroupSubject
            {
                StudyGroupId = studyGroup.Id,
                SubjectId = subject.Id,
                StudySemester = StudySemester.Y21H1,
                // LectorMentorId = teacher?.Id
            };

            Student student = testCase.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);
            const int pointCount = 10;

            //TODO: refactor
            var activity = new SubjectActivity { GroupSubject = groupSubject, StudentId = student.Id, Points = pointCount, Student = student};
            List<StudyLeaderboardRowDto> studyLeaderboardRowDtos = GetStudentsRatings(StudySearchParametersDto.ForGroup(studyGroup.Id), new List<SubjectActivity> {activity});

            StudyLeaderboardRowDto? studentResult = studyLeaderboardRowDtos.FirstOrDefault(slr => slr.Student.Id == student.Id);
            Assert.IsNotNull(studentResult);
            Assert.AreEqual(pointCount, studentResult.Activity);
        }

        public List<StudyLeaderboardRowDto> GetStudentsRatings(StudySearchParametersDto searchParametersDto, List<SubjectActivity> activities)
        {
            if (searchParametersDto.CourseId is null && searchParametersDto.GroupId is null)
                throw new IwentysExecutionException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");

            return activities
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDto(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Skip(searchParametersDto.Skip)
                .Take(searchParametersDto.Take)
                .ToList();
        }
    }
}