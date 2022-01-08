namespace Iwentys.Common
{
    public partial class InnerLogicException
    {
        public static class AssignmentExceptions
        {
            public static InnerLogicException IsNotAssignmentCreator(int assignmentId, int userId)
            {
                return new InnerLogicException($"Assignment {assignmentId} doesn't belong to this user {userId}");
            }

            public static InnerLogicException IsAlreadyCompleted(int assignmentId)
            {
                return new InnerLogicException($"Assignment {assignmentId} is already completed");
            }

            public static InnerLogicException IsNotCompleted(int assignmentId)
            {
                return new InnerLogicException($"Assignment {assignmentId} is not completed");
            }
        }
    }
}
