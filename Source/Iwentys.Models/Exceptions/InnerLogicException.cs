namespace Iwentys.Models.Exceptions
{
    public class InnerLogicException : IwentysException
    {
        public InnerLogicException(string message) : base(message)
        {
            
        }

        public static InnerLogicException NotEnoughPermission(int userId) => new InnerLogicException($"Not enough user permission for user {userId}");
    }
}