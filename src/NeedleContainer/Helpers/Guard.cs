namespace Needle.Helpers
{
    using System;

    internal static class Guard
    {
        internal static void ThrowIfNullArgument(object o, string paramName) 
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName); 
            }
        }
    }
}
