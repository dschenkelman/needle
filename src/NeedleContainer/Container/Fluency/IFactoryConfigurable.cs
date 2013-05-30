namespace Needle.Container.Fluency
{
    using System;

    public interface IFactoryConfigurable<in T> : IHideObjectMethods
    {
        ICommittableIdentifiableLifetimeable WithFactory(Func<T> factoryMethod);
    }
}
