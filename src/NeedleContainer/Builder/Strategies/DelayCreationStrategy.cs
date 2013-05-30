namespace Needle.Builder.Strategies
{
    using System;
    using Needle.Container;

    public class DelayCreationStrategy : BaseBuilderStrategy
    {
        public override BuildingStep BuildingStep
        {
            get { return BuildingStep.PreBuilding; }
        }

        public override void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container)
        {
            if (buildStatus.BuildCompleted) 
            {
                return;
            }

            if (!CanResolveType(buildStatus.TypeToBuild))
            {
                return;
            }

            Type underlyingType = buildStatus.TypeToBuild.GetGenericArguments()[0];
                
            Type helperType = typeof(GetterHelper<>).MakeGenericType(underlyingType);
            Type delegateType = typeof(Func<>).MakeGenericType(underlyingType);

            var getterHelper = Activator.CreateInstance(helperType, new object[] { container });
            buildStatus.FactoryMethod = () => Delegate.CreateDelegate(delegateType, getterHelper, helperType.GetMethod("Get"));
            buildStatus.BuildCompleted = true;
        }

        private static bool CanResolveType(Type typeToBuild)
        {
            return typeToBuild.IsGenericType && typeToBuild.GetGenericTypeDefinition() == typeof(Func<>);
        }

        private class GetterHelper<TItem>
        {
            private readonly INeedleContainer container;

            public GetterHelper(INeedleContainer container)
            {
                this.container = container;
            }

            public TItem Get()
            {
                return this.container.Get<TItem>();
            }
        }
    }
}
