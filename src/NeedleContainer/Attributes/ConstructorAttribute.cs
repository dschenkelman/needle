namespace Needle.Attributes
{
    using System;

    /// <summary>
    /// This attribute is use to mark a constructor as the one to be used to perform dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed class ConstructorAttribute : Attribute
    {
    }
}
