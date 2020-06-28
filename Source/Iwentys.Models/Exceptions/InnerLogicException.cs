using System;

namespace Iwentys.Models.Exceptions
{
    public class InnerLogicException : IwentysException
    {
        public InnerLogicException(string message) : base(message)
        {
            
        }

        public static InnerLogicException NotEnoughPermission(int userId) => new InnerLogicException($"Not enough user permission for user {userId}");
        public static InnerLogicException NotSupportedEnumValue<T>(T value) where T : Enum => new InnerLogicException($"Unsupported [{value.GetType()}] type: {value}");
        public static InnerLogicException NotEnoughBarsPoints() => new InnerLogicException("User don't have enough points for this operation.");
    }
}