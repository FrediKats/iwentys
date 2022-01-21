using System.Runtime.Serialization;

namespace Iwentys.EntityManager.Common;

[Serializable]
public class IwentysExecutionException : IwentysException
{
    public IwentysExecutionException()
    {
    }

    public IwentysExecutionException(string message) : base(message)
    {
    }

    public IwentysExecutionException(string message, Exception inner) : base(message, inner)
    {
    }

    protected IwentysExecutionException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}