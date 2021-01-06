using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.SubjectAssignments.Domain
{
    public class SubjectTeacher
    {
        public SubjectTeacher(GroupSubject subject, IwentysUser teacher)
        {
            if (!IsTeacher(subject, teacher))
                throw InnerLogicException.StudyExceptions.UserIsNotTeacher(teacher.Id);
            
            User = teacher;
        }

        public SubjectTeacher(Subject subject, IwentysUser teacher)
        {
            if (!IsTeacher(subject, teacher))
                throw InnerLogicException.StudyExceptions.UserIsNotTeacher(teacher.Id);

            User = teacher;
        }

        public IwentysUser User { get; set; }

        public static bool IsTeacher(GroupSubject subject, IwentysUser teacher) => subject.LectorTeacherId == teacher.Id || subject.PracticeTeacherId == teacher.Id;
        public static bool IsTeacher(Subject subject, IwentysUser teacher) => subject.GroupSubjects.Any(gs => gs.LectorTeacherId == teacher.Id || gs.PracticeTeacherId == teacher.Id);
    }

    public static class SubjectTeacherExtensions
    {
        public static SubjectTeacher EnsureIsTeacher(this IwentysUser teacher, GroupSubject subject) => new SubjectTeacher(subject, teacher);
        public static SubjectTeacher EnsureIsTeacher(this IwentysUser teacher, Subject subject) => new SubjectTeacher(subject, teacher);
    }
}