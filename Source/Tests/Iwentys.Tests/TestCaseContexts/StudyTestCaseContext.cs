using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;

namespace Iwentys.Tests.TestCaseContexts
{
    public class StudyTestCaseContext
    {
        public Student WithNewStudentAsStudent(StudyGroup studyGroup)
        {
            StudentCreateArguments createArguments = UsersFaker.Instance.Students.Generate();
            createArguments.Id = UsersFaker.Instance.GetIdentifier();
            createArguments.GroupId = studyGroup.Id;

            var student = Student.Create(createArguments);
            studyGroup.AddStudent(student);
            return student;
        }
    }
}