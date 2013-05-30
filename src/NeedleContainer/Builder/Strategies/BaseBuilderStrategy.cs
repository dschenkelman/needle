namespace Needle.Builder.Strategies
{
    using Needle.Container;
    using Needle.Helpers;

    public abstract class BaseBuilderStrategy : IBuilderStrategy
    {
        public abstract BuildingStep BuildingStep { get; }

        public int CompareTo(IBuilderStrategy other)
        {
            Guard.ThrowIfNullArgument(other, "other");

            return this.BuildingStep.CompareTo(other.BuildingStep);
        }
        
        public abstract void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container);
    }
}
