using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study
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

        public static bool IsTeacher(GroupSubject subject, IwentysUser teacher)
        {
            return subject.LectorTeacherId == teacher.Id || subject.PracticeTeacherId == teacher.Id;
        }

        public static bool IsTeacher(Subject subject, IwentysUser teacher)
        {
            return subject.GroupSubjects.Any(gs => gs.LectorTeacherId == teacher.Id || gs.PracticeTeacherId == teacher.Id);
        }
    }

    public static class SubjectTeacherExtensions
    {
        public static SubjectTeacher EnsureIsTeacher(this IwentysUser teacher, GroupSubject subject)
        {
            return new SubjectTeacher(subject, teacher);
        }

        public static SubjectTeacher EnsureIsTeacher(this IwentysUser teacher, Subject subject)
        {
            return new SubjectTeacher(subject, teacher);
        }
    }
}