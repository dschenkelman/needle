namespace Needle.Builder
{
    using System;
    using System.Reflection;

    public interface IBuildStatus
    {
        /// <summary>
        /// Gets the type to build.
        /// </summary>
        /// <value>The type to build.</value>
        Type TypeToBuild { get;  }

        /// <summary>
        /// Gets or sets the constructor method.
        /// </summary>
        /// <value>The constructor method.</value>
        ConstructorInfo ConstructorMethod { get;  set; }

        /// <summary>
        /// Gets or sets the constructor dependencies factory methods.
        /// </summary>
        /// <value>The constructor dependencies.</value>
        Func<object>[] ConstructorDependenciesFactories { get; set; }

        /// <summary>
        /// Gets the factory method to build the type from the <see cref="TypeToBuild"/> property.
        /// </summary>
        /// <value>The factory method.</value>
        Func<object> FactoryMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the build process has been completed.
        /// </summary>
        /// <value><c>true</c> if the build process has been completed; otherwise, <c>false</c>.</value>
        bool BuildCompleted { get; set; }

        /// <summary> Adds a factory to construct an instance of the property with the provided name..</summary>
        /// <param name="propertyName">The property's name.</param>
        /// <param name="factory">The factory to construct the property's instance.</param>
        void AddDependencyPropertyFactory(string propertyName, Func<object> factory);

        /// <summary> Gets the factory method to build the property with the provided name.</summary>
        /// <param name="propertyName">The property's name.</param>
        /// <returns>The factory method.</returns>
        Func<object> GetDependencyPropertyFactory(string propertyName);
    }
}
