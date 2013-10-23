namespace Needle.Builder
{
    using System;
    using Needle.Builder.Strategies;
    using Needle.Container;
    using Needle.Helpers;

    /// <summary>
    /// Builder class used by the NeedleContainer to create and inject objects when resolving dependencies.
    /// </summary>
    internal class NeedleBuilder : IBuilder
    {
        private readonly FactoryRegistry factoryRegistry;
        private readonly BuilderStrategyCollection builderStrategies;
        
        internal NeedleBuilder()
        {
            this.factoryRegistry = new FactoryRegistry();
            this.builderStrategies = new BuilderStrategyCollection();
            this.AddDefautStrategies();
        }

        /// <summary>
        /// Builds an object of the specified type.
        /// </summary>
        /// <param name="typeMapping">The type to build.</param>
        /// <param name="container">The container that was requested to build the type.</param>
        /// <returns></returns>
        public object Build(TypeMapping typeMapping, INeedleContainer container)
        {
            return this.InstantiateObjectWithConstructorDependencies(typeMapping, container);
        }

        public void AddFactoryMethod(TypeMapping typeMapping, Factory<object> factory)
        {
            this.factoryRegistry.AddFactory(typeMapping, factory);
        }

        private object InstantiateObjectWithConstructorDependencies(TypeMapping typeMapping, INeedleContainer container)
        {
            if (!this.factoryRegistry.HasFactory(typeMapping))
            {
                IBuildStatus buildStatus = new BuildStatus(typeMapping.ToType);
                foreach (IBuilderStrategy strategy in this.builderStrategies)
                {
                    strategy.ExecuteStrategy(buildStatus, container);
                }

                this.factoryRegistry.AddFactory(typeMapping, buildStatus.FactoryMethod);
            }

            return this.factoryRegistry.GetFactory(typeMapping).Invoke();
        }

        private void AddDefautStrategies()
        {
            PreBuildCompositeBuilderStrategy preBuilderStrategy = new PreBuildCompositeBuilderStrategy();
            preBuilderStrategy.AddStrategy(new DelayCreationStrategy())
                .AddStrategy(new GetAllStrategy());

            this.builderStrategies.AddStrategy(new ConstructorStrategy())
                .AddStrategy(new ConstructorDependenciesStrategy())
                .AddStrategy(preBuilderStrategy)
                .AddStrategy(new PropertyDependenciesStrategy());
        }
    }
}
