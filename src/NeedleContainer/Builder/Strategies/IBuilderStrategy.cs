namespace Needle.Builder.Strategies
{
    using System;
    using Needle.Container;

    public interface IBuilderStrategy : IComparable<IBuilderStrategy>
    {
        BuildingStep BuildingStep { get; }

        void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container);
    }
}
