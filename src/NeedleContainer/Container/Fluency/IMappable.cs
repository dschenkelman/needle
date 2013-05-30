namespace Needle.Container.Fluency
{
    using System;

    public interface IMappable<in T> : IHideObjectMethods
    {
        ICommittableIdentifiableLifetimeableFactoryConfigurable<TTo> To<TTo>() where TTo : class, T;

        ICommittableIdentifiableLifetimeableFactoryConfigurable<object> To(Type type);
    }
}