namespace Iwentys.EntityManager.Common;

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

    public static InnerLogicException NotSupportedEnumValue<T>(T value)
        where T : Enum
    {
        return new InnerLogicException($"Unsupported [{value.GetType()}] type: {value}");
    }
}