using Needle.Container;

namespace Needle.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class BuildStatus : IBuildStatus
    {
        private readonly Dictionary<string, Factory<object>> dependencyPropertiesFactories;

        private Factory<object> factoryMethod;

        public BuildStatus(Type typeToBuild)
        {
            this.TypeToBuild = typeToBuild;
            this.BuildCompleted = false;
            this.dependencyPropertiesFactories = new Dictionary<string, Factory<object>>();
        }

        /// <summary>
        /// Gets or sets the constructor method.
        /// </summary>
        /// <value>The constructor method.</value>
        public ConstructorInfo ConstructorMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the constructor dependencies factory methods.
        /// </summary>
        /// <value>The constructor dependencies.</value>
        public Factory<object>[] ConstructorDependenciesFactories
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets the factory method to build the type from the <see cref="TypeToBuild"/> property.
        /// </summary>
        /// <value>The factory method.</value>
        public Factory<object> FactoryMethod
        {
            get 
            {
                if (this.factoryMethod == null && this.ConstructorDependenciesFactories != null && this.ConstructorMethod != null) 
                {
                    this.factoryMethod = () =>
                    {
                        var dependenciesInstances = new List<object>();
                        foreach (var dependencyFactory in this.ConstructorDependenciesFactories)
                        {
                            dependenciesInstances.Add(dependencyFactory.Invoke());
                        }

                        object instance = this.ConstructorMethod.Invoke(dependenciesInstances.ToArray());

                        foreach (var kvp in this.dependencyPropertiesFactories)
                        {
                            string propertyName = kvp.Key;
                            Factory<object> factory = kvp.Value;
                            instance.GetType().GetProperty(propertyName).SetValue(instance, factory(), null);
                        }

                        return instance;
                    };
                }

                return this.factoryMethod;
            }

            set 
            {
                this.factoryMethod = value;
            }
        }

        /// <summary>
        /// Gets the type to build.
        /// </summary>
        /// <value>The type to build.</value>
        public Type TypeToBuild { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the build process has been completed.
        /// </summary>
        /// <value><c>true</c> if the build process has been completed; otherwise, <c>false</c>.</value>
        public bool BuildCompleted { get; set; }

        /// <summary> Adds a factory to construct an instance of the property with the provided name..</summary>
        /// <param name="propertyName">The property's name.</param>
        /// <param name="factory">The factory to construct the property's instance.</param>
        public void AddDependencyPropertyFactory(string propertyName, Factory<object> factory)
        {
            this.dependencyPropertiesFactories[propertyName] = factory;
        }

        /// <summary> Gets the factory method to build the property with the provided name.</summary>
        /// <param name="propertyName">The property's name.</param>
        /// <returns>The factory method.</returns>
        public Factory<object> GetDependencyPropertyFactory(string propertyName)
        {
            return this.dependencyPropertiesFactories[propertyName];
        }
    }
}