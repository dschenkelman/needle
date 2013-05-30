namespace Needle.Builder.Strategies
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Needle.Attributes;
    using Needle.Container;
    using Needle.Helpers;

    public class PropertyDependenciesStrategy : BaseBuilderStrategy
    {
        public override BuildingStep BuildingStep
        {
            get { return BuildingStep.PropertyDependenciesResolution; }
        }

        public override void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container)
        {
            Guard.ThrowIfNullArgument(buildStatus, "buildStatus");

            var properties = buildStatus.TypeToBuild.GetProperties();
            var dependencyProperties = properties.Where(p => p.GetCustomAttributes(typeof(DependencyAttribute), false).Count() != 0);

            foreach (var property in dependencyProperties)
            {
                object[] attributes = property.GetCustomAttributes(typeof(DependencyAttribute), false);
                if (attributes.Length != 1) 
                {
                    continue;
                }

                Func<object> dependencyFactory = ResolveDependency(container, property);
                buildStatus.AddDependencyPropertyFactory(property.Name, dependencyFactory);
            }   
        }

        private static Func<object> ResolveDependency(INeedleContainer container, PropertyInfo property)
        {
            DependencyAttribute attribute = (DependencyAttribute)property.GetCustomAttributes(typeof(DependencyAttribute), false)[0];
            Type underlyingType = property.PropertyType;
            Type helperType = typeof(GetterHelper<>).MakeGenericType(underlyingType);
            Type delegateType = typeof(Func<>).MakeGenericType(underlyingType);
            var getterHelper = Activator.CreateInstance(helperType, new object[] { container, attribute.Id });
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
