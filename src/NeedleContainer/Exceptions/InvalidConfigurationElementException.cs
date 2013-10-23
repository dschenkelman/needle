namespace Needle.Exceptions
{
    using System;

    public class InvalidConfigurationElementException : Exception
    {
        public InvalidConfigurationElementException(string message) : base(message)
        {
        }
    }
}
