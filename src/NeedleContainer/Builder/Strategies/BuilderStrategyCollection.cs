using System.Linq;

namespace Needle.Builder.Strategies
{
    using System.Collections;
    using System.Collections.Generic;

    using Needle.Helpers;

    public class BuilderStrategyCollection : IBuilderStrategyCollection
    {
        private readonly List<IBuilderStrategy> builderStrategies;
        
        public BuilderStrategyCollection()
        {
            this.builderStrategies = new List<IBuilderStrategy>();
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

            var existingStrategy = this.builderStrategies.FirstOrDefault(s => s.BuildingStep == strategy.BuildingStep);

            if (existingStrategy != null)
            {
                this.builderStrategies.Remove(existingStrategy);
            }

            this.builderStrategies.Add(strategy);

            return this;
        }

        public IBuilderStrategyCollection RemoveStrategy(IBuilderStrategy strategy)
        {
            Guard.ThrowIfNullArgument(strategy, "strategy");
            
            this.builderStrategies.Remove(strategy);

            return this;
        }

        public IBuilderStrategy GetStrategy(BuildingStep step) 
        {
            return this.builderStrategies.FirstOrDefault(s => s.BuildingStep == step);
        }

        #region IEnumerable Members
        public IEnumerator<IBuilderStrategy> GetEnumerator()
        {
            this.builderStrategies.Sort((s1, s2) => s1.CompareTo(s2));
            return this.builderStrategies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
