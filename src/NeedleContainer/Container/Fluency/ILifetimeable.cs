namespace Needle.Container.Fluency
{
    public interface ILifetimeable : IHideObjectMethods
    {
        ICommittableIdentifiable UsingLifetime(RegistrationLifetime lifetime);
    }
}
