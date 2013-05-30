namespace Needle.Helpers
{
    using System;
    using System.Collections.Generic;

    using Needle.Container;
    using Needle.Exceptions;

    internal class FactoryRegistry
    {
        private readonly IDictionary<TypeMapping, Func<object>> factories;

        public FactoryRegistry()
        {
            this.factories = new Dictionary<TypeMapping, Func<object>>();
        }

        internal void AddFactory(TypeMapping typeMapping, Func<object> factory)
        {
            this.factories[typeMapping] = factory;
        }

        internal Func<object> GetFactory(TypeMapping typeMapping)
        {
            Func<object> factory;
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
