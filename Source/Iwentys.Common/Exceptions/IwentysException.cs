using System;
using System.Runtime.Serialization;

namespace Iwentys.Common
{
    public class IwentysException : Exception
    {
        public IwentysException()
        {
        }

        public IwentysException(string message) : base(message)
        {
        }

        public IwentysException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IwentysException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}