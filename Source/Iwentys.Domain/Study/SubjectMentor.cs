using Iwentys.Common;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study
{
    public class SubjectMentor
    {
        public SubjectMentor(GroupSubject subject, IwentysUser mentor)
        {
            if (!subject.HasMentorPermission(mentor))
                throw InnerLogicException.StudyExceptions.UserIsNotMentor(mentor.Id);

            Mentor = mentor;
        }

        public SubjectMentor(Subject subject, IwentysUser mentor)
        {
            if (!subject.HasMentorPermission(mentor))
                throw InnerLogicException.StudyExceptions.UserIsNotMentor(mentor.Id);

            Mentor = mentor;
        }

        public IwentysUser Mentor { get; }
    }

    public static class SubjectTeacherExtensions
    {
        public static SubjectMentor EnsureIsMentor(this IwentysUser teacher, GroupSubject subject)
        {
            return new SubjectMentor(subject, teacher);
        }

        public static SubjectMentor EnsureIsMentor(this IwentysUser teacher, Subject subject)
        {
            return new SubjectMentor(subject, teacher);
        }
    }
}