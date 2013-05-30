namespace Needle.Container.Fluency
{
    public interface IStorageIdentifiable : IHideObjectMethods
    {
        ICommittable WithId(string id);
    }

    public interface IMappingIdentifiable : IHideObjectMethods
    {
        ICommittableLifetimeable WithId(string id);
    }
}
