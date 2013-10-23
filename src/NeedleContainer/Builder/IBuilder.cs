namespace Needle.Builder
{
    using Needle.Container;

    internal interface IBuilder
    {
        /// <summary>
        /// Builds an object of the specified type.
        /// </summary>
        /// <param name="typeMapping">The type mapping to build.</param>
        /// <param name="container">The container that was requested to build the type.</param>
        /// <returns>The built object, with dependencies injected.</returns>
        object Build(TypeMapping typeMapping, INeedleContainer container);

        void AddFactoryMethod(TypeMapping typeMapping, Factory<object> factory);
    }
}
