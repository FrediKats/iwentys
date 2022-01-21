namespace Iwentys.EntityManager.Common;

public partial class InnerLogicException
{
    public static class StudyExceptions
    {
        public static InnerLogicException UserIsNotMentor(int userId)
        {
            return new InnerLogicException($"User is not teacher. Not enough permission. User {userId}");
        }

        public static InnerLogicException UserIsNotGroupAdmin(int userId)
        {
            return new InnerLogicException($"User is not group admin. Not enough permission. User {userId}");
        }
    }
}