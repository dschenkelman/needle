namespace Needle.Builder.Strategies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using Needle.Container;

    public class PreBuildCompositeBuilderStrategy : IBuilderStrategy, IBuilderStrategyCollection
    {
        private readonly List<IBuilderStrategy> strategies;

        public PreBuildCompositeBuilderStrategy()
        {
            this.strategies = new List<IBuilderStrategy>();
        }

        public BuildingStep BuildingStep
        {
            get { return BuildingStep.PreBuilding; }
        }

        public int StrategyCount
        {
            get { return this.strategies.Count; }
        }

        public void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container)
        {
            foreach (IBuilderStrategy strategy in this)
            {
                strategy.ExecuteStrategy(buildStatus, container);
            }
        }

        public int CompareTo(IBuilderStrategy other)
        {
            return this.BuildingStep.CompareTo(other.BuildingStep);
        }

        public IBuilderStrategyCollection AddStrategy(IBuilderStrategy strategy) 
        {
            if (strategy.BuildingStep != this.BuildingStep)
            {
                throw new ArgumentException("The strategy's buiding step must be of the same type of the composite strategy's step.");
            }

            this.strategies.Add(strategy);
            return this;
        }

        public IBuilderStrategyCollection RemoveStrategy(IBuilderStrategy strategy)
        {
            this.strategies.Remove(strategy);
            return this;
        }

        public IEnumerator<IBuilderStrategy> GetEnumerator()
        {
            return this.strategies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
