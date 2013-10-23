namespace Needle.Container.Fluency
{
    public interface IFactoryConfigurable<in T> : IHideObjectMethods
    {
        ICommittableIdentifiableLifetimeable WithFactory(Factory<T> factoryMethod);
    }
}
