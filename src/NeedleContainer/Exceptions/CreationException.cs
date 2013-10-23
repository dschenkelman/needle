namespace Needle.Exceptions
{
    using System;

    public class CreationException : Exception
    {
        public CreationException()
        {
        }

        public CreationException(string message) : base(message)
        {
        }
    }
}
