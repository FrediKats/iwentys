namespace Iwentys.Common;

public partial class InnerLogicException
{
    public static class SubjectAssignmentException
    {
        public static InnerLogicException StudentIsNotAssignedToSubject(int studentId, int subjectId)
        {
            throw new InnerLogicException($"Student is not assigned to subject. Student: {studentId}, Subject:{subjectId}");
        }
    }
}