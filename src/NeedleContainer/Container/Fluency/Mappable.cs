namespace Needle.Container.Fluency
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Needle.Properties;

    internal class Mappable<T> : IMappable<T> where T : class
    {
        private readonly TypeMapping mapping;

        private readonly InstanceRegistration registration;

        private readonly Action<TypeMapping, InstanceRegistration, Func<object>> commitAction;

        public Mappable(TypeMapping mapping, InstanceRegistration registration, Action<TypeMapping, InstanceRegistration, Func<object>> commitAction)
        {
            this.mapping = mapping;
            this.registration = registration;
            this.commitAction = commitAction;
        }

        public ICommittableIdentifiableLifetimeableFactoryConfigurable<TTo> To<TTo>() where TTo : class, T
        {
            this.UpdateMappingWithToType(typeof(TTo));
            return this.CreateRegistrationProxy<TTo>();
        }

        public ICommittableIdentifiableLifetimeableFactoryConfigurable<object> To(Type type)
        {
            this.UpdateMappingWithToType(type);

            return this.CreateRegistrationProxy<object>();
        }

        private void UpdateMappingWithToType(Type type)
        {
            if (!this.mapping.FromType.IsAssignableFrom(type))
            {
                throw new ArgumentException(Resources.IncorrectTypeMappingTypes);
            }

            this.mapping.ToType = type;
        }

        private ICommittableIdentifiableLifetimeableFactoryConfigurable<TTo> CreateRegistrationProxy<TTo>() where TTo : class
        {
            return new MappingRegistrationProxy<TTo>(this.mapping, this.registration, this.commitAction);
        }
    }
}