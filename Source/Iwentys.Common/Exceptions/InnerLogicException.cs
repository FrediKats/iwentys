using System;

namespace Iwentys.Common.Exceptions
{
    //TODO: split inner classed to separate files and make it partial
    public partial class InnerLogicException : IwentysException
    {
        public InnerLogicException(string message) : base(message)
        {
        }

        public InnerLogicException()
        {
        }

        public InnerLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static InnerLogicException NotEnoughPermissionFor(int userId)
        {
            return new InnerLogicException($"Not enough user permission for user {userId}");
        }

        public static InnerLogicException NotSupportedEnumValue<T>(T value) where T : Enum
        {
            return new InnerLogicException($"Unsupported [{value.GetType()}] type: {value}");
        }

        public static InnerLogicException NotEnoughBarsPoints()
        {
            return new InnerLogicException("Student don't have enough points for this operation.");
        }
    }
}