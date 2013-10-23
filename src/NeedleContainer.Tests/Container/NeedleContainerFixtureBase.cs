namespace Needle.Tests.Container
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Needle.Container;

    using MockWithMultiplePropertyDependencies = Needle.Tests.Mocks.MockWithPropertyDependencies.MockWithMultiplePropertyDependencies;
    using MockWithNamedPropertyDependency = Needle.Tests.Mocks.MockWithPropertyDependencies.MockWithNamedPropertyDependency;
    using MockWithoutDependencies = Needle.Tests.Mocks.MocksWithoutDependencies.MockWithoutDependencies;
    using MockWithoutDependenciesDefaultConstructor = Needle.Tests.Mocks.MocksWithoutDependencies.MockWithoutDependenciesDefaultConstructor;
    using MockWithPropertyDependency = Needle.Tests.Mocks.MockWithPropertyDependencies.MockWithPropertyDependency;
    using MockWithPropertyFactoryDependency = Needle.Tests.Mocks.MockWithPropertyDependencies.MockWithPropertyFactoryDependency;
    using MockWithPropertyGetAllDependency = Needle.Tests.Mocks.MockWithPropertyDependencies.MockWithPropertyGetAllDependency;

    [TestClass]
    public abstract class NeedleContainerFixtureBase
    {
        protected INeedleContainer Container { get; set; }

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.Container = new NeedleContainer();
        }

        [TestMethod]
        public void GettingObjectWithoutDependenciesReturnsCorrectObject()
        {
            MockWithoutDependencies firstObject = this.GetFromContainer<MockWithoutDependencies>();
            Assert.IsTrue(firstObject.Constructed);
        }

        [TestMethod]
        public void GettingObjectWithoutDependenciesByInterfaceMapping()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependenciesDefaultConstructor>();
            IMockWithoutDependencies firstObject = this.GetFromContainer<IMockWithoutDependencies>();
            Assert.IsNotNull(firstObject);
            Assert.IsTrue(firstObject.Constructed);
        }

        [TestMethod]
        public void GettingObjectWithThroughInterfaceReturnsCorrectObject()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>();
            var obj = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.IsTrue(obj.Constructed);
            Assert.IsTrue(obj.GetType() == typeof(MockWithoutDependencies));
        }

        [TestMethod]
        public void GettingObjectWithMultipleConstructorCallsAttributedConstructor()
        {
            MockWithManyConstructors obj = this.GetFromContainer<MockWithManyConstructors>();
            Assert.IsTrue(obj.AttributedConstructorCalled);
        }

        [TestMethod]
        public void GettingObjectWithNoConstructorDefinedUsesDefaultConstructor()
        {
            MockWithUndefinedConstructor obj = this.GetFromContainer<MockWithUndefinedConstructor>();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(MockWithUndefinedConstructor));
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void GettingObjectsWithMultipleInjectionConstructorsThrowsException()
        {
            this.GetFromContainer<MockWithMultipleInjectionConstructors>();
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void GettingObjectThroughNonMappedInterfaceThrowsException()
        {
            this.GetFromContainer<IMockWithoutDependencies>();
        }

        [TestMethod]
        public void AddsDependenciesToObjectWithObjectDependency()
        {
            var obj = this.GetFromContainer<MockWithObjectDependency>();

            Assert.IsNotNull(obj.Dependency);
        }

        [TestMethod]
        public void GettingObjectWithDependencyTwiceGetsDifferentInstanceOfNotStoredDependency() 
        {
            var obj = this.GetFromContainer<MockWithObjectDependency>();
            var obj2 = this.GetFromContainer<MockWithObjectDependency>();

            Assert.AreNotSame(obj.Dependency, obj2.Dependency);
        }

        [TestMethod]
        public void AddsDependenciesToObjectDependencies()
        {
            var obj = this.GetFromContainer<MockWithDependencyThatHasDependency>();
            Assert.IsTrue(obj.Dependency.Dependency.Constructed);
        }

        [TestMethod]
        public void CanInjectMultipleDependenciesToObject()
        {
            var obj = this.GetFromContainer<MockWithThreeDependencies>();
            Assert.IsNotNull(obj.Dependency1);
            Assert.IsNotNull(obj.Dependency2);
            Assert.IsNotNull(obj.Dependency3);
        }

        [TestMethod]
        public void MappingTypesWithSingletonLifetimeAlwaysReturnsSameInstance()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Singleton);
            var firstObject = this.GetFromContainer<IMockWithoutDependencies>();
            var secondObject = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.AreSame(firstObject, secondObject);
        }

        [TestMethod]
        public void MappingTypesWithTransientLifetimeReturnsDifferentInstances()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Transient);
            var firstObject = this.GetFromContainer<IMockWithoutDependencies>();
            var secondObject = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.AreNotSame(firstObject, secondObject);
        }

        [TestMethod]
        public void MappingTypesWithoutLifetimeUsesTransient()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>();
            var firstObject = this.GetFromContainer<IMockWithoutDependencies>();
            var secondObject = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.AreNotSame(firstObject, secondObject);
        }

        [TestMethod]
        public void CanGetInstanceFromContainerStoredAsNonAbstractType()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer(mock);

            var storedInstance = this.GetFromContainer<MockWithoutDependencies>();

            Assert.AreSame(mock, storedInstance);
        }

        [TestMethod]
        public void StoringWithAnAlreadyStoredInstanceStepsOnTheOldInstance()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer(mock);

            var secondMock = new MockWithoutDependencies();
            this.StoreInContainer(secondMock);

            var storedInstance = this.GetFromContainer<MockWithoutDependencies>();

            Assert.AreSame(secondMock, storedInstance);
        }

        [TestMethod]
        public void CanGetInstanceFromContainerStoredAsAbstractType()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(mock);

            var storedInstance = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.AreSame(mock, storedInstance);
        }

        [TestMethod]
        public void CanGetInstanceStoredWithName()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(mock, "FirstMock");

            var secondMock = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(secondMock, "SecondMock");

            var firstInstance = this.GetFromContainer<IMockWithoutDependencies>("FirstMock");
            var secondInstance = this.GetFromContainer<IMockWithoutDependencies>("SecondMock");

            Assert.AreNotSame(firstInstance, secondInstance);
            Assert.AreSame(mock, firstInstance);
            Assert.AreSame(secondMock, secondInstance);
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void GettingNotNamedObjectThroughAbstractTypeAfterStoringNamedInstanceShouldThrowException()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(mock, "FirstMock");

            this.GetFromContainer<IMockWithoutDependencies>();
        }

        [TestMethod]
        [ExpectedException(typeof(RegistrationNotFoundException))]
        public void GettingInstanceOfClassUsingNonRegisteredIdThrowsException()
        {
            this.GetFromContainer<MockWithoutDependencies>("NonRegisteredId");
        }

        [TestMethod]
        public void CanGetAllStoredNamedInstancesOfANonAbstractType()
        {
            List<MockWithoutDependencies> mocks = new List<MockWithoutDependencies>();

            for (int i = 0; i < 10; i++)
            {
                var mock = new MockWithoutDependencies();
                mocks.Add(mock);
                this.StoreInContainer(mock, string.Format("Mock{0}", i));
            }

            IEnumerable<MockWithoutDependencies> storedInstances =
                this.GetAllFromContainer<MockWithoutDependencies>();

            Assert.IsNotNull(storedInstances);

            Assert.AreEqual(10, storedInstances.Count());
            foreach (var mock in mocks)
            {
                Assert.IsTrue(storedInstances.Contains(mock));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void GetAllForNotStoredInstancesThrowsCreationException()
        {
            this.GetAllFromContainer<MockWithoutDependencies>();
        }

        [TestMethod]
        public void CanGetAllStoredNamedInstancesOfAnAbstractType()
        {
            List<MockWithoutDependencies> mocks = new List<MockWithoutDependencies>();

            for (int i = 0; i < 10; i++)
            {
                var mock = new MockWithoutDependencies();
                mocks.Add(mock);
                this.StoreInContainer<IMockWithoutDependencies>(mock, string.Format("Mock{0}", i));
            }

            IEnumerable<IMockWithoutDependencies> storedInstances = this.GetAllFromContainer<IMockWithoutDependencies>();

            Assert.IsNotNull(storedInstances);

            Assert.AreEqual(10, storedInstances.Count());
            foreach (var mock in mocks)
            {
                Assert.IsTrue(storedInstances.Contains(mock));
            }
        }

        [TestMethod]
        public void CanInjectNamedRegisteredInstanceWhenRequested()
        {
            var obj = new MockDependency();
            this.StoreInContainer(obj, "Mock");

            var mockWithNamedDependency = this.GetFromContainer<MockWithNamedDependency>();

            Assert.IsNotNull(mockWithNamedDependency);
            Assert.AreSame(obj, mockWithNamedDependency.Dependency);
        }

        [TestMethod]
        public void CanInjectRegisteredSingleton()
        {
            var obj = new MockDependency();
            this.StoreInContainer(obj);

            var mockWithObjectDependency = this.GetFromContainer<MockWithObjectDependency>();

            Assert.IsNotNull(mockWithObjectDependency);
            Assert.AreSame(obj, mockWithObjectDependency.Dependency);
        }

        [TestMethod]
        public void CanChangeInstanceRegistrationAnResolvingInstancesIsCorrect()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>();
            IMockWithoutDependencies resolvedObject = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.IsInstanceOfType(resolvedObject, typeof(MockWithoutDependencies));
            Assert.IsTrue(resolvedObject.Constructed);

            // Change mapping
            MapInContainer<IMockWithoutDependencies, OtherMockWithoutDependencies>();
            resolvedObject = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.IsInstanceOfType(resolvedObject, typeof(OtherMockWithoutDependencies));
            Assert.IsTrue(resolvedObject.Constructed);
        }

        [TestMethod]
        public void TakesLessTimeToResolveObjectSecondTime()
        {
            Stopwatch watch = Stopwatch.StartNew();
            this.GetFromContainer<MockWithDependencyThatHasDependency>();
            watch.Stop();

            Stopwatch secondWatch = Stopwatch.StartNew();
            this.GetFromContainer<MockWithDependencyThatHasDependency>();
            secondWatch.Stop();

            Assert.IsTrue(secondWatch.Elapsed.Ticks < watch.Elapsed.Ticks);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallingStoreWithNullInstanceThrows() 
        {
            this.StoreInContainer<MockWithoutDependencies>(null);
        }

        [TestMethod]
        public void CanGetFuncOfTypeInjected()
        {
            Func<MockWithoutDependencies> buildFunc = this.GetFromContainer<Func<MockWithoutDependencies>>();
            var mock = buildFunc();
            Assert.IsInstanceOfType(mock, typeof(MockWithoutDependencies));
            Assert.IsTrue(mock.Constructed);
        }

        [TestMethod]
        public void CanResolveUsingIEnumerableOfType()
        {
            List<MockWithoutDependencies> mocks = new List<MockWithoutDependencies>();

            for (int i = 0; i < 10; i++)
            {
                var mock = new MockWithoutDependencies();
                mocks.Add(mock);
                this.StoreInContainer<MockWithoutDependencies>(mock, string.Format("Mock{0}", i.ToString()));
            }

            IEnumerable<MockWithoutDependencies> storedInstances = this.GetFromContainer<IEnumerable<MockWithoutDependencies>>();

            Assert.IsNotNull(storedInstances);

            Assert.AreEqual(10, storedInstances.Count());
            foreach (var mock in mocks)
            {
                Assert.IsTrue(storedInstances.Contains(mock));
            }
        }

        [TestMethod]
        public void CanCombineDelayInstantiationAndIEnumerableOfType()
        {
            List<MockWithoutDependencies> mocks = new List<MockWithoutDependencies>();

            for (int i = 0; i < 10; i++)
            {
                var mock = new MockWithoutDependencies();
                mocks.Add(mock);
                this.StoreInContainer<MockWithoutDependencies>(mock, string.Format("Mock{0}", i.ToString()));
            }

            Func<IEnumerable<MockWithoutDependencies>> delayedFactory = this.GetFromContainer<Func<IEnumerable<MockWithoutDependencies>>>();
            
            IEnumerable<MockWithoutDependencies> storedInstances = delayedFactory.Invoke();

            Assert.IsNotNull(storedInstances);

            Assert.AreEqual(10, storedInstances.Count());
            foreach (var mock in mocks)
            {
                Assert.IsTrue(storedInstances.Contains(mock));
            }
        }

        [TestMethod]
        public void CanGetNeedleContainerFromContainerThroughConcreteType() 
        {
            NeedleContainer instance = this.GetFromContainer<NeedleContainer>();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void CanGetNeedleContainerFromContainerThroughContainerInterface()
        {
            INeedleContainer instance = this.GetFromContainer<INeedleContainer>();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        [ExpectedException(typeof(CreationException))]
        public void GettingAnotherInstanceOfClassThroughDifferentIdWithoutMappingThrowsException()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(mock, "MockRegistration");

            this.GetFromContainer<IMockWithoutDependencies>();
        }

        [TestMethod]
        public void CanGetAnotherInstanceOfClassOnceNamedRegistrationIsStored()
        {
            var mock = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(mock, "MockRegistration");

            var firstInstance = this.GetFromContainer<IMockWithoutDependencies>("MockRegistration");

            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>();
            var secondInstance = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.AreNotSame(firstInstance, secondInstance);
            Assert.AreSame(mock, firstInstance);
        }

        [TestMethod]
        public void CanMapTypesUsingIdGettingTransientMapping() 
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>("MockId");

            var instance = this.GetFromContainer<IMockWithoutDependencies>("MockId");
            var secondInstance = this.GetFromContainer<IMockWithoutDependencies>("MockId");

            Assert.IsNotNull(instance);
            Assert.AreNotSame(instance, secondInstance);
        }

        [TestMethod]
        public void CanMapTypesUsingIdAndSingletonLifetime()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Singleton, "MockId");

            var instance = this.GetFromContainer<IMockWithoutDependencies>("MockId");
            var secondInstance = this.GetFromContainer<IMockWithoutDependencies>("MockId");

            Assert.IsNotNull(instance);
            Assert.AreSame(instance, secondInstance);
        }

        [TestMethod]
        public void CanMapTypesUsingDifferentIdsAndScopes() 
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Singleton, "Singleton");
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Transient, "Transient");

            var singleton = this.GetFromContainer<IMockWithoutDependencies>("Singleton");
            var transient = this.GetFromContainer<IMockWithoutDependencies>("Transient");

            Assert.AreNotSame(singleton, transient);

            var sameSingleton = this.GetFromContainer<IMockWithoutDependencies>("Singleton");

            Assert.AreSame(singleton, sameSingleton);
        }

        [TestMethod]
        public void GetAllForTypeReturnsRegisteredMappings() 
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Singleton, "Singleton");
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Transient, "Transient");

            var singleton = this.GetFromContainer<IMockWithoutDependencies>("Singleton");

            var allRegistrations = this.GetAllFromContainer<IMockWithoutDependencies>();

            Assert.AreEqual(2, allRegistrations.Count());
            Assert.IsTrue(allRegistrations.Contains(singleton));
        }

        [TestMethod]
        public void GetAllForTypeReturnsRegisteredMappingsAndStoreInstances()
        {
            var instance = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(instance);
            this.MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Singleton, "Singleton");
            this.MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Transient, "Transient");

            var singleton = this.GetFromContainer<IMockWithoutDependencies>("Singleton");

            var allRegistrations = this.GetAllFromContainer<IMockWithoutDependencies>();

            Assert.AreEqual(3, allRegistrations.Count());
            Assert.IsTrue(allRegistrations.Contains(instance));
            Assert.IsTrue(allRegistrations.Contains(singleton));
        }

        [TestMethod]
        public void CanReplaceStoredInstanceWithTypeMappingUsingSameId() 
        {
            var instance = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(instance, "MockId");
            var obtainedInstance = this.GetFromContainer<IMockWithoutDependencies>("MockId");

            Assert.AreSame(instance, obtainedInstance);

            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Transient, "MockId");
            obtainedInstance = this.GetFromContainer<IMockWithoutDependencies>("MockId");

            Assert.AreNotSame(instance, obtainedInstance);
        }

        [TestMethod]
        public void CanReplaceTypeMappingWithStoredInstanceUsingSameId()
        {
            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(RegistrationLifetime.Transient, "MockId");
            var obtainedInstance = this.GetFromContainer<IMockWithoutDependencies>("MockId");
            
            var instance = new MockWithoutDependencies();
            this.StoreInContainer<IMockWithoutDependencies>(instance, "MockId");
            var secondObtainedInstance = this.GetFromContainer<IMockWithoutDependencies>("MockId");

            Assert.AreNotSame(obtainedInstance, secondObtainedInstance);
            Assert.AreSame(instance, secondObtainedInstance);
        }

        [TestMethod]
        public void CanInjectSingleDependencyWithoutDependenciesToObject()
        {
            var instance = this.GetFromContainer<MockWithPropertyDependency>();
            Assert.IsNotNull(instance.Dependency);
        }

        [TestMethod]
        public void CanInjectPropertyAndConstructorDependenciesSimultaneously() 
        {
            var instance = this.GetFromContainer<MockWithPropertyAndConstructorDependency>();
            Assert.IsNotNull(instance.ConstructorDependency);
            Assert.IsNotNull(instance.PropertyDependency);
        }

        [TestMethod]
        public void DependenciesInjectedThroughConstructorAndPropertiesCorrectlyInjectStoredSingleton()
        {
            var mock = new MockDependency();
            this.StoreInContainer(mock);
            
            var instance = this.GetFromContainer<MockWithPropertyAndConstructorDependency>();

            Assert.AreSame(mock, instance.ConstructorDependency);
            Assert.AreSame(mock, instance.PropertyDependency);
        }

        [TestMethod]
        public void DependenciesInjectedThroughConstructorAndPropertiesCorrectlyInjectMappedSingleton()
        {
            MapInContainer<MockDependency, MockDependency>(RegistrationLifetime.Singleton);

            var instance = this.GetFromContainer<MockWithPropertyAndConstructorDependency>();

            Assert.AreSame(instance.PropertyDependency, instance.ConstructorDependency);
        }

        [TestMethod]
        public void CanInjectDependencyRequestingFactoryOfType() 
        {
            var instance = this.GetFromContainer<MockWithPropertyFactoryDependency>();
            
            MockDependency dep = instance.PropertyDependency();
            Assert.IsNotNull(dep);
        }

        [TestMethod]
        public void CanInjectDependencyRequestingAllRegistrationsOfType()
        {
            List<MockDependency> mocks = new List<MockDependency>();

            for (int i = 0; i < 10; i++)
            {
                var mock = new MockDependency();
                mocks.Add(mock);
                this.StoreInContainer(mock, string.Format("Mock{0}", i));
            }

            var instance = this.GetFromContainer<MockWithPropertyGetAllDependency>();

            var mockCollection = instance.PropertyDependency;

            Assert.IsNotNull(mockCollection);

            Assert.AreEqual(10, mockCollection.Count());
            foreach (var mock in mocks)
            {
                Assert.IsTrue(mockCollection.Contains(mock));
            }
        }

        [TestMethod]
        public void CanInjectDependenciesToMultipleProperties() 
        {
            var mock = this.GetFromContainer<Needle.Tests.Mocks.MockWithPropertyDependencies.MockWithMultiplePropertyDependencies>();

            Assert.IsNotNull(mock.FirstDependency);
            Assert.IsNotNull(mock.SecondDependency);
            Assert.IsNotNull(mock.SecondDependency.Dependency);
        }

        [TestMethod]
        public void CanInjectNamedPropertyDependencies() 
        {
            var mock = new MockWithoutDependencies();

            this.StoreInContainer(mock, "DependencyId");

            var instance = this.GetFromContainer<MockWithNamedPropertyDependency>();

            Assert.AreSame(mock, instance.Dependency);
        }

        [TestMethod]
        public void CanProvideFactoryForMappingWhenUsingTransientLifetime()
        {
            var mock = new MockWithoutDependencies();

            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(() => mock);

            var obtainedMock = this.GetFromContainer<IMockWithoutDependencies>();

            Assert.AreSame(obtainedMock, mock);
        }

        [TestMethod]
        public void CanProvideFactoryForNamedMappingButStillResolvesTheType()
        {
            var mock = new MockWithoutDependencies();

            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>("Registration", () => mock);
            
            var obtainedMock = this.GetFromContainer<IMockWithoutDependencies>("Registration");
            var obtainedMock2 = this.GetFromContainer<MockWithoutDependencies>();

            Assert.AreNotSame(obtainedMock, obtainedMock2);
        }

        [TestMethod]
        public void CanProvideFactoryForNotNamedMappingButStillResolveTheType()
        {
            var mock = new MockWithoutDependencies();

            MapInContainer<IMockWithoutDependencies, MockWithoutDependencies>(() => mock);

            var obtainedMock = this.GetFromContainer<IMockWithoutDependencies>();
            var obtainedMock2 = this.GetFromContainer<MockWithoutDependencies>();

            Assert.AreNotSame(obtainedMock, obtainedMock2);
        }

        #region Abstract Methods

        protected abstract T GetFromContainer<T>();

        protected abstract T GetFromContainer<T>(string id);

        protected abstract IEnumerable<T> GetAllFromContainer<T>();

        protected abstract void MapInContainer<TFrom, TTo>() 
            where TFrom : class
            where TTo : class, TFrom;

        protected abstract void MapInContainer<TFrom, TTo>(string id)
            where TFrom : class
            where TTo : class, TFrom;

        protected abstract void MapInContainer<TFrom, TTo>(string id, Factory<TTo> factory) 
            where TFrom : class
            where TTo : class, TFrom;

        protected abstract void MapInContainer<TFrom, TTo>(Factory<TTo> factory)
            where TFrom : class
            where TTo : class, TFrom;

        protected abstract void MapInContainer<TFrom, TTo>(RegistrationLifetime lifetime) 
            where TFrom : class
            where TTo : class, TFrom;

        protected abstract void MapInContainer<TFrom, TTo>(RegistrationLifetime lifetime, string id) 
            where TFrom : class
            where TTo : class, TFrom;

        protected abstract void StoreInContainer<T>(T instance);

        protected abstract void StoreInContainer<T>(T instance, string id);

        #endregion
    }
}
