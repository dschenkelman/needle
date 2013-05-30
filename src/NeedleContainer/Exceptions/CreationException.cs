namespace Needle.Exceptions
{
    using System;

    [Serializable]
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
