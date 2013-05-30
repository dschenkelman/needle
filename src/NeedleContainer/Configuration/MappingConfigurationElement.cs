namespace Needle.Configuration
{
    using System;
    using Needle.Container;

    public class MappingConfigurationElement
    {
        public MappingConfigurationElement(string fromType, string toType)
            : this(fromType, toType, "Transient", string.Empty)
        {
        }

        public MappingConfigurationElement(string fromType, string toType, string lifeTime, string registrationId)
        {
            this.RegistrationId = registrationId;
            this.FromType = Type.GetType(fromType);
            this.ToType = Type.GetType(toType);
            this.Lifetime = (RegistrationLifetime)Enum.Parse(typeof(RegistrationLifetime), lifeTime, true);
        }

        public Type FromType { get; private set; }

        public Type ToType { get; private set; }

        public RegistrationLifetime Lifetime { get; private set; }

        public string RegistrationId { get; private set; }
    }
}
