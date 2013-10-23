namespace Needle.Tests.Container
{
    using System;
    using System.Collections.Generic;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Needle.Container;

    [TestClass]
    public class NeedleContainerGenericsFixture : NeedleContainerFixtureBase
    {
        protected override T GetFromContainer<T>()
        {
            return this.Container.Get<T>();
        }

        protected override T GetFromContainer<T>(string id)
        {
            return this.Container.Get<T>(id);
        }

        protected override IEnumerable<T> GetAllFromContainer<T>()
        {
            return this.Container.GetAll<T>();
        }

        protected override void MapInContainer<TFrom, TTo>()
        {
            this.Container.Map<TFrom>().To<TTo>().Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(string id)
        {
            this.Container.Map<TFrom>().To<TTo>().WithId(id).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(string id, Factory<TTo> factory)
        {
            this.Container.Map<TFrom>().To<TTo>().WithFactory(factory).WithId(id).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(Factory<TTo> factory)
        {
            this.Container.Map<TFrom>().To<TTo>().WithFactory(factory).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(RegistrationLifetime lifetime)
        {
            this.Container.Map<TFrom>().To<TTo>().UsingLifetime(lifetime).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(RegistrationLifetime lifetime, string id)
        {
            this.Container.Map<TFrom>().To<TTo>().UsingLifetime(lifetime).WithId(id).Commit();
        }

        protected override void StoreInContainer<T>(T instance)
        {
            this.Container.Store<T>(instance).Commit();
        }

        protected override void StoreInContainer<T>(T instance, string id)
        {
            this.Container.Store<T>(instance).WithId(id).Commit();
        }
    }
}
