namespace Needle.Container.Fluency
{
    using System;

    using Needle.Properties;

    internal abstract class RegistrationProxy<T> : ICommittableIdentifiableLifetimeableFactoryConfigurable<T>,
        ICommittableIdentifiable, ICommittableLifetimeable
    {
        protected RegistrationProxy(TypeMapping mapping, InstanceRegistration registration)
        {
            this.Mapping = mapping;
            this.Registration = registration;
            this.IsCommited = false;
        }

        protected TypeMapping Mapping { get; set; }

        protected InstanceRegistration Registration { get; set; }

        protected bool IsCommited { get; set; }

        public void Commit()
        {
            if (this.IsCommited)
            {
                throw new InvalidOperationException(Resources.CannotCallCommitMoreThanOnce);
            }
            
            this.IsCommited = true;

            this.InvokeCommitAction();
        }

        ICommittable IStorageIdentifiable.WithId(string id)
        {
            return this.WithIdInternal(id);
        }

        ICommittableLifetimeable IMappingIdentifiable.WithId(string id)
        {
            return this.WithIdInternal(id);
        }

        public ICommittableIdentifiable UsingLifetime(RegistrationLifetime lifetime)
        {
            this.Mapping.Lifetime = lifetime;

            return this;
        }

        public virtual ICommittableIdentifiableLifetimeable WithFactory(Func<T> factory)
        {
            return this;
        }

        protected abstract void InvokeCommitAction();

        private ICommittableLifetimeable WithIdInternal(string id)
        {
            if (this.Registration != null)
            {
                // being used to store types. if mapping, registration is null
                this.Registration.Id = id;
            }

            this.Mapping.Id = id;
            return this;
        }
    }
}