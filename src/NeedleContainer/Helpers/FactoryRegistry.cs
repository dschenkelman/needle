namespace Needle.Helpers
{
    using System.Collections.Generic;

    using Needle.Container;
    using Needle.Exceptions;

    internal class FactoryRegistry
    {
        private readonly IDictionary<TypeMapping, Factory<object>> factories;

        public FactoryRegistry()
        {
            this.factories = new Dictionary<TypeMapping, Factory<object>>();
        }

        internal void AddFactory(TypeMapping typeMapping, Factory<object> factory)
        {
            this.factories[typeMapping] = factory;
        }

        internal Factory<object> GetFactory(TypeMapping typeMapping)
        {
            Factory<object> factory;
            if (!this.factories.TryGetValue(typeMapping, out factory))
            {
                throw new FactoryNotFoundException();
            }

            return factory;
        }

        internal bool HasFactory(TypeMapping typeMapping)
        {
            return this.factories.ContainsKey(typeMapping);
        }
    }
}
