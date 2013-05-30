namespace Needle.Container.Fluency
{
    using System;

    internal class MappingRegistrationProxy<T> : RegistrationProxy<T> where T : class
    {
        private readonly Action<TypeMapping, InstanceRegistration, Func<object>> commitAction;
        private Func<object> factoryMethod;

        public MappingRegistrationProxy(
            TypeMapping mapping,
            InstanceRegistration registration,
            Action<TypeMapping, InstanceRegistration, Func<object>> commitAction) : base(mapping, registration)
        {
            this.commitAction = commitAction;
        }

        public override ICommittableIdentifiableLifetimeable WithFactory(Func<T> factory)
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
