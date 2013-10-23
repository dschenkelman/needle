namespace Needle.Container.Fluency
{
    using System;

    internal class MappingRegistrationProxy<T> : RegistrationProxy<T> where T : class
    {
        private readonly Action<TypeMapping, InstanceRegistration, Factory<object>> commitAction;
        private Factory<object> factoryMethod;

        public MappingRegistrationProxy(
            TypeMapping mapping,
            InstanceRegistration registration,
            Action<TypeMapping, InstanceRegistration, Factory<object>> commitAction)
            : base(mapping, registration)
        {
            this.commitAction = commitAction;
        }

        public override ICommittableIdentifiableLifetimeable WithFactory(Factory<T> factory)
        {
            this.factoryMethod = factory;

            return base.WithFactory(factory);
        }

        protected override void InvokeCommitAction()
        {
            this.commitAction.Invoke(this.Mapping, this.Registration, this.factoryMethod);
        }
    }
}
