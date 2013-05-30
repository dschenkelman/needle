namespace Needle.Builder.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Needle.Container;

    public class GetAllStrategy : BaseBuilderStrategy
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
            var getterHelper = Activator.CreateInstance(helperType, new object[] { container });

            MethodInfo method = getterHelper.GetType().GetMethod("GetAll");

            buildStatus.FactoryMethod = () => method.Invoke(getterHelper, null);
            buildStatus.BuildCompleted = true;
        }

        private static bool CanResolveType(Type typeToBuild) 
        {
            return typeToBuild.IsGenericType && typeToBuild.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private class GetterHelper<TItem>
        {
            private readonly INeedleContainer container;

            public GetterHelper(INeedleContainer container)
            {
                this.container = container;
            }

            public IEnumerable<TItem> GetAll()
            {
                return this.container.GetAll<TItem>();
            }
        }
    }
}
