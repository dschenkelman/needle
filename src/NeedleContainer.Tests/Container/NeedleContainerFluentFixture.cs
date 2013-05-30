namespace Needle.Tests.Container
{
    using System;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Needle.Container;

    using MockWithoutDependencies = Needle.Tests.Mocks.MocksWithoutDependencies.MockWithoutDependencies;

    [TestClass]
    public class NeedleContainerFluentFixture
    {
        private INeedleContainer container;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.container = new NeedleContainer();
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void NotCallingCommitDoesNotStoreInstance()
        {
            var instance = new MockWithoutDependencies();
            this.container.Store<IMockWithoutDependencies>(instance);
            this.container.Get<IMockWithoutDependencies>();
        }

        [TestMethod]
        public void CallingCommitStoresInstance()
        {
            var instance = new MockWithoutDependencies();
            this.container.Store<IMockWithoutDependencies>(instance).Commit();

            Assert.AreSame(instance, this.container.Get<IMockWithoutDependencies>());
        }

        [TestMethod]
        public void CallingWithIdAndThenCommitStoresInstancWithId()
        {
            var instance = new MockWithoutDependencies();
            this.container.Store<IMockWithoutDependencies>(instance).WithId("RegistrationId").Commit();
            
            Assert.AreSame(instance, this.container.Get<IMockWithoutDependencies>("RegistrationId"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallingCommitMoreThanOnceThrows()
        {
            var instance = new MockWithoutDependencies();
            var committable = this.container.Store<IMockWithoutDependencies>(instance);
            committable.Commit();
            committable.Commit();
        }

        [TestMethod]
        public void CallingCommitDoesMapTypes()
        {
            this.container.Map<IMockWithoutDependencies>().To<MockWithoutDependencies>().Commit();

            var instance = this.container.Get<IMockWithoutDependencies>();

            Assert.IsInstanceOfType(instance, typeof(MockWithoutDependencies));
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void NotCallingCommitDoesNotMapTypes()
        {
            this.container.Map<IMockWithoutDependencies>().To<MockWithoutDependencies>();

            var instance = this.container.Get<IMockWithoutDependencies>();
        }

        [TestMethod]
        public void CallingWithIdAndThenCommitMapsTypesUsingId()
        {
            this.container.Map<IMockWithoutDependencies>().To<MockWithoutDependencies>().WithId("TestId").Commit();
            var instance = this.container.Get<IMockWithoutDependencies>("TestId");

            Assert.IsInstanceOfType(instance, typeof(MockWithoutDependencies));
        }

        [TestMethod]
        public void CallingUsingLifetimeWithSingletonAndThenCommitMapsTypesUsingSingletonLifetime()
        {
            this.container.Map<IMockWithoutDependencies>().To<MockWithoutDependencies>().UsingLifetime(RegistrationLifetime.Singleton).Commit();

            var instance = this.container.Get<IMockWithoutDependencies>();
            var secondInstace = this.container.Get<IMockWithoutDependencies>();

            Assert.AreSame(instance, secondInstace);
        }

        [TestMethod]
        public void CanSetLifetimeAndIdInAnyGivenOrder()
        {
            this.container.Map<IMockWithoutDependencies>()
                .To<MockWithoutDependencies>()
                .WithId("FirstRegistration")
                .UsingLifetime(RegistrationLifetime.Singleton)
                .Commit();

            this.container.Map<IMockWithoutDependencies>()
                .To<MockWithoutDependencies>()
                .UsingLifetime(RegistrationLifetime.Singleton)
                .WithId("SecondRegistration")
                .Commit();

            var firstInstance = this.container.Get<IMockWithoutDependencies>("FirstRegistration");
            var secondInstance = this.container.Get<IMockWithoutDependencies>("SecondRegistration");

            Assert.IsInstanceOfType(firstInstance, typeof(MockWithoutDependencies));
            Assert.IsInstanceOfType(secondInstance, typeof(MockWithoutDependencies));
        }
    }
}
