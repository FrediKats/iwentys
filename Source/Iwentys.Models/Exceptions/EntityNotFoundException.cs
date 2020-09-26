using System;
using Iwentys.Models.ExceptionMessages;

namespace Iwentys.Models.Exceptions
{
    public class EntityNotFoundException : IwentysException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException() : base()
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static EntityNotFoundException Create<TType, TKey>(TType type, TKey key)
        {
            return new EntityNotFoundException($"[{type}] Entity was not found for key: [{key}]");
        }

        public static EntityNotFoundException PinnedRepoWasNotFound(int pinnedRepoId)
        {
            return new EntityNotFoundException(string.Format(GuildExceptions.PinnedRepoWasNotFound, pinnedRepoId));
        }
    }
}