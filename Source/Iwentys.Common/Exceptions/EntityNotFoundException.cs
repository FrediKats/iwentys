using System;

namespace Iwentys.Common
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

        public static EntityNotFoundException Create<TType>(TType type)
        {
            return new EntityNotFoundException($"[{type}] Entity was not found.");
        }

        public static EntityNotFoundException Create<TType, TKey>(TType type, TKey key)
        {
            return new EntityNotFoundException($"[{type}] Entity was not found for key: [{key}]");
        }

        public static EntityNotFoundException Create<TType, TKey1, TKey2>(TType type, TKey1 key1, TKey2 key2)
        {
            return new EntityNotFoundException($"[{type}] Entity was not found for key: [{key1}, {key2}]");
        }
    }
}