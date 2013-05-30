namespace Needle.Tests.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Needle.Container;
    using Needle.Tests.Mocks;

    [TestClass]
    public class NeedleContainerFixture : NeedleContainerFixtureBase
    {
        // the following test only makes sense with non generic values for the first parameter
        // otherwise the constraints do the job for us
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotMapTypesWhenToTypeIsNotChildOfFromType()
        {
            this.Container.Map(typeof(IMockWithoutDependencies)).To<MockDependency>();
        }

        protected override T GetFromContainer<T>()
        {
            return (T)this.Container.Get(typeof(T));
        }

        protected override T GetFromContainer<T>(string id)
        {
            return (T)this.Container.Get(typeof(T), id);
        }

        protected override IEnumerable<T> GetAllFromContainer<T>()
        {
            return this.Container.GetAll(typeof(T)).Cast<T>();
        }

        protected override void MapInContainer<TFrom, TTo>()
        {
            this.Container.Map(typeof(TFrom)).To(typeof(TTo)).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(string id)
        {
            this.Container.Map(typeof(TFrom)).To(typeof(TTo)).WithId(id).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(string id, Func<TTo> factory)
        {
            this.Container.Map(typeof(TFrom)).To(typeof(TTo)).WithFactory(factory).WithId(id).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(Func<TTo> factory)
        {
            this.Container.Map(typeof(TFrom)).To(typeof(TTo)).WithFactory(factory).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(RegistrationLifetime lifetime)
        {
            this.Container.Map(typeof(TFrom)).To(typeof(TTo)).UsingLifetime(lifetime).Commit();
        }

        protected override void MapInContainer<TFrom, TTo>(RegistrationLifetime lifetime, string id)
        {
            this.Container.Map(typeof(TFrom)).To(typeof(TTo)).UsingLifetime(lifetime).WithId(id).Commit();
        }

        protected override void StoreInContainer<T>(T instance)
        {
            this.Container.Store(typeof(T), instance).Commit();
        }

        protected override void StoreInContainer<T>(T instance, string id)
        {
            this.Container.Store(typeof(T), instance).WithId(id).Commit();
        }
    }
}
