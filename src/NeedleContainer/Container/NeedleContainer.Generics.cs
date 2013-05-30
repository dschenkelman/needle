namespace Needle.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Needle.Container.Fluency;

    public partial class NeedleContainer : INeedleContainer
    {
        public T Get<T>()
        {
            return (T)this.Get(typeof(T));
        }

        public T Get<T>(string id)
        {
            return (T)this.Get(typeof(T), id);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.GetAll(typeof(T)).Cast<T>();
        }

        public IMappable<TFrom> Map<TFrom>() where TFrom : class
        {
            var mapping = this.GenericMapping<TFrom>(typeof(TFrom));
            return mapping;
        }

        public ICommittableIdentifiable Store<T>(T instance)
        {
            return this.Store(typeof(T), instance);
        }
    }
}