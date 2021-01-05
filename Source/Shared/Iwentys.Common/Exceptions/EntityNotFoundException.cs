using System;

namespace Iwentys.Common.Exceptions
{
    public class EntityNotFoundException : IwentysException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static EntityNotFoundException Create<TType, TKey>(TType type, TKey key)
        {
            return new EntityNotFoundException($"[{type}] Entity was not found for key: [{key}]");
        }
    }
}