namespace Needle.Container.Fluency
{
    public interface ICommittableIdentifiableLifetimeableFactoryConfigurable<in T> : ICommittableIdentifiableLifetimeable, IFactoryConfigurable<T>
    {
    }
}
