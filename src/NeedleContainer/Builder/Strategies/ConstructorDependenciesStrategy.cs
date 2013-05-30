namespace Needle.Builder.Strategies
{
    using System;
    using System.Linq;
    using System.Reflection;
    
    using Needle.Attributes;
    using Needle.Container;
    using Needle.Helpers;

    public class ConstructorDependenciesStrategy : BaseBuilderStrategy
    {
        public override BuildingStep BuildingStep
        {
            get { return BuildingStep.ConstructorDependenciesResolution; }
        }
        
        public override void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container)
        {
            Guard.ThrowIfNullArgument(buildStatus, "buildStatus");

            if (buildStatus.BuildCompleted) 
            {
                return;
            }

            buildStatus.ConstructorDependenciesFactories = buildStatus.ConstructorMethod
                .GetParameters()
                .Select(parameterInfo => ResolveDependency(parameterInfo, container))
                .ToArray();
        }

        private static Func<object> ResolveDependency(ParameterInfo parameterInfo, INeedleContainer container)
        {
            string id = string.Empty;
            object[] attributes = parameterInfo.GetCustomAttributes(typeof(DependencyAttribute), false);
            if (attributes.Length == 1)
            {
                DependencyAttribute attribute = attributes[0] as DependencyAttribute;
                if (attribute != null)
                {
                    id = attribute.Id;
                }
            }

            Type underlyingType = parameterInfo.ParameterType;
            Type helperType = typeof(GetterHelper<>).MakeGenericType(underlyingType);
            Type delegateType = typeof(Func<>).MakeGenericType(underlyingType);
            var getterHelper = Activator.CreateInstance(helperType, new object[] { container, id });
            return Delegate.CreateDelegate(delegateType, getterHelper, helperType.GetMethod("Get")) as Func<object>;
        }

        private class GetterHelper<TItem>
        {
            private readonly INeedleContainer container;
            private readonly string id;

            public GetterHelper(INeedleContainer container, string id)
            {
                this.container = container;
                this.id = id;
            }

            public TItem Get()
            {
                return this.container.Get<TItem>(this.id);
            }
        }
    }
}
