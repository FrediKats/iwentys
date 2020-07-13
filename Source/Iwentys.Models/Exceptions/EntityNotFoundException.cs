namespace Iwentys.Models.Exceptions
{
    public class EntityNotFoundException : IwentysException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public static EntityNotFoundException Create<TType, TKey>(TType type, TKey key)
        {
            return new EntityNotFoundException($"[{type}] Entity was not found for key: [{key}]");
        }
    }
}