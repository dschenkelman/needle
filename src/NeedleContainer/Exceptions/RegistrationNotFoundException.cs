namespace Needle.Exceptions
{
    using System;

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
