namespace Needle.Container
{
    public enum RegistrationLifetime
    {
        // The container does not hold a reference to the created object and creates a new instance each time one is requested
        Transient,

        // The container holds a reference to the first instance created an always returns the same instance
        Singleton
    }
}