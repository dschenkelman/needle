namespace Needle.Builder.Strategies
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    
    using Needle.Attributes;
    using Needle.Container;
    using Needle.Exceptions;
    using Needle.Helpers;
    using Needle.Properties;

    public class ConstructorStrategy : BaseBuilderStrategy
    {
        public override BuildingStep BuildingStep
        {
            get { return BuildingStep.ConstructorDetermination; }
        }

        public override void ExecuteStrategy(IBuildStatus buildStatus, INeedleContainer container)
        {
            Guard.ThrowIfNullArgument(buildStatus, "buildStatus");

            if (buildStatus.BuildCompleted) 
            {
                return;
            }
            
            Type type = buildStatus.TypeToBuild;
            ConstructorInfo injectionConstructor = null;
            ConstructorInfo defaultConstructor = null;
            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (var constructorInfo in constructors)
            {
                if (!constructorInfo.GetParameters().Any())
                {
                    defaultConstructor = constructorInfo;
                }

                if (constructorInfo.GetCustomAttributes(typeof(ConstructorAttribute), false).Length != 1)
                {
                    continue;
                }

                if (injectionConstructor != null)
                {
                    throw new CreationException(string.Format(
                        CultureInfo.CurrentCulture, 
                        Resources.MultipleInjectionConstructors, 
                        type.FullName));
                }

                injectionConstructor = constructorInfo;
            }

            if (injectionConstructor == null && defaultConstructor != null)
            {
                injectionConstructor = defaultConstructor;
            }

            if (injectionConstructor == null)
            {
                throw new CreationException(string.Format(
                    CultureInfo.CurrentCulture, 
                    Resources.UndefinedInjectionConstructor,
                    type.FullName));
            }

            buildStatus.ConstructorMethod = injectionConstructor;
        }
    }
}