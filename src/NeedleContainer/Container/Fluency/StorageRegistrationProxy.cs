namespace Needle.Container.Fluency
{
    using System;

    internal class StorageRegistrationProxy : RegistrationProxy<object>
    {
        private readonly Action<TypeMapping, InstanceRegistration> commitAction;

        internal StorageRegistrationProxy(TypeMapping mapping, InstanceRegistration registration, Action<TypeMapping, InstanceRegistration> commitAction) : base(mapping, registration)
        {
            this.commitAction = commitAction;
        }

        protected override void InvokeCommitAction()
        {
            this.commitAction.Invoke(this.Mapping, this.Registration);
        }
    }
}
