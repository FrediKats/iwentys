namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class Assignment
        {
            public static InnerLogicException IsNotAssignmentCreator(int assignmentId, int userId)
            {
                return new InnerLogicException($"Assignment {assignmentId} doesn't belong to this user {userId}");
            }

            public static InnerLogicException IsAlreadyCompleted(int assignmentId)
            {
                return new InnerLogicException($"Assignment {assignmentId} is already completed");
            }
        } 
    }
}
