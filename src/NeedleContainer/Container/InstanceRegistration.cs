namespace Needle.Container
{
    internal class InstanceRegistration
    {
        internal InstanceRegistration(string id, object instance)
        {
            this.Id = id;
            this.Instance = instance;
        }

        internal string Id { get; set; }

        internal object Instance { get; private set; }
    }
}
