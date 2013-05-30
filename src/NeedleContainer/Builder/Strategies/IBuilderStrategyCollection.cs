namespace Needle.Builder.Strategies
{
    using System.Collections.Generic;

    public interface IBuilderStrategyCollection : IEnumerable<IBuilderStrategy>
    {
        int StrategyCount { get; }

        IBuilderStrategyCollection AddStrategy(IBuilderStrategy strategy);

        IBuilderStrategyCollection RemoveStrategy(IBuilderStrategy strategy);
    }
}
