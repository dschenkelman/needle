namespace Needle.Attributes
{
    using System;

    /// <summary>
    /// This attribute is used to mark a named dependency in an injection constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DependencyAttribute : Attribute
    {
        public DependencyAttribute() : this(string.Empty)
        {
        }

        public DependencyAttribute(string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }
    }
}
