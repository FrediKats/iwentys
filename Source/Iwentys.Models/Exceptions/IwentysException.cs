using System;

namespace Iwentys.Models.Exceptions
{
    public class IwentysException : Exception
    {
        public IwentysException(string message) : base(message)
        {
        }
    }
}