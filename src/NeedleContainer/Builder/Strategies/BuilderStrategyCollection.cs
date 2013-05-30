namespace Needle.Builder.Strategies
{
    using System.Collections;
    using System.Collections.Generic;

    using Needle.Helpers;

    public class BuilderStrategyCollection : IBuilderStrategyCollection
    {
        private readonly SortedList<BuildingStep, IBuilderStrategy> builderStrategies;
        
        public BuilderStrategyCollection()
        {
            this.builderStrategies = new SortedList<BuildingStep, IBuilderStrategy>();
        }

        public int StrategyCount 
        { 
            get 
            {
                return this.builderStrategies.Count;
            }
        }

        public IBuilderStrategyCollection AddStrategy(IBuilderStrategy strategy)
        {
            Guard.ThrowIfNullArgument(strategy, "strategy");

            this.builderStrategies[strategy.BuildingStep] = strategy;

            return this;
        }

        public IBuilderStrategyCollection RemoveStrategy(IBuilderStrategy strategy)
        {
            Guard.ThrowIfNullArgument(strategy, "strategy");
            
            this.builderStrategies.Remove(strategy.BuildingStep);

            return this;
        }

        public IBuilderStrategy GetStrategy(BuildingStep step) 
        {
            return this.builderStrategies[step];
        }

        #region IEnumerable Members
        public IEnumerator<IBuilderStrategy> GetEnumerator()
        {
            return this.builderStrategies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
