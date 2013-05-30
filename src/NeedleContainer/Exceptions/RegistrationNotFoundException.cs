namespace Needle.Exceptions
{
    using System;

    [Serializable]
    public class RegistrationNotFoundException : Exception
    {
        public RegistrationNotFoundException()
        {
        }

        public RegistrationNotFoundException(string message) : base(message)
        {
        }
    }
}
