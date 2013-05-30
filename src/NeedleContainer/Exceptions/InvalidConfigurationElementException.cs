namespace Needle.Exceptions
{
    using System;

    [Serializable]
    public class InvalidConfigurationElementException : Exception
    {
        public InvalidConfigurationElementException(string message) : base(message)
        {
        }
    }
}
