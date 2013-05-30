namespace Needle.Tests.Builder
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Moq;
    using Needle.Builder;

    [TestClass]
    public class BuildStatusFixture
    {
        private BuildStatus status;

        [TestInitialize]
        public void Initialize() 
        {
            this.status = new BuildStatus(typeof(MockWithObjectDependency));
        }

        [TestMethod]
        public void CreatingBuildStatusSetsBuildCompletedToFalse() 
        {
            Assert.IsFalse(this.status.BuildCompleted);
        }

        [TestMethod]
        public void CreatingBuildStatusSetsTypeToBuildAttribute()
        {
            Assert.AreEqual(typeof(MockWithObjectDependency), this.status.TypeToBuild);
        }

        [TestMethod]
        public void SettingConstrutorParametersReturnsNullFactoryMethod() 
        {
            var mockDependency = new Mock<Type>();
            this.status.ConstructorDependenciesFactories = new Func<object>[] { () => new MockDependency() };
            Assert.IsNull(this.status.FactoryMethod);
        }

        [TestMethod]
        public void SettingConstructorMethodReturnsNullFactoryMethod()
        {
            this.status.ConstructorMethod = typeof(MockWithObjectDependency).GetConstructors()[0];
            Assert.IsNull(this.status.FactoryMethod);
        }

        [TestMethod]
        public void SettingConstructorMethodAndConstructorParametersCreateFactoryMethod()
        {
            var constructorDependency = new MockDependency();

            this.status.ConstructorDependenciesFactories = new Func<object>[] { () => constructorDependency };

            this.status.ConstructorMethod = typeof(MockWithObjectDependency).GetConstructors()[0];

            object obj = this.status.FactoryMethod.Invoke();

            Assert.IsInstanceOfType(obj, typeof(MockWithObjectDependency));

            var mock = obj as MockWithObjectDependency;
            Assert.AreSame(constructorDependency, mock.Dependency);
        }
        
        [TestMethod]
        public void SettingConstructorMethodPropertyFactoriesAndConstructorParametersCreateFactoryMethod()
        {
            var constructorDependency = new MockDependency();
            var propertyDependency = new MockDependency();
            
            this.status.ConstructorDependenciesFactories = new Func<object>[] { () => constructorDependency };
            Func<object> propertyFactory = new Func<object>(() => propertyDependency);
            
            this.status.ConstructorMethod = typeof(MockWithPropertyAndConstructorDependency).GetConstructors()[0];
            this.status.AddDependencyPropertyFactory("PropertyDependency", propertyFactory);
            
            object obj = this.status.FactoryMethod.Invoke();
            
            Assert.IsInstanceOfType(obj, typeof(MockWithPropertyAndConstructorDependency));
            
            var mock = obj as MockWithPropertyAndConstructorDependency;
            Assert.AreSame(constructorDependency, mock.ConstructorDependency);
            Assert.AreSame(propertyDependency, mock.PropertyDependency);
        }

        [TestMethod]
        public void CanAddFactoryForPropertyUsingPropertyName() 
        {
            var factory = new Func<object>(() => new MockDependency());
            this.status.AddDependencyPropertyFactory("PropertyName", factory);
            Func<object> obtainedFactory = this.status.GetDependencyPropertyFactory("PropertyName");

            Assert.AreEqual(factory, obtainedFactory);
            
            var instance = factory();
            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(MockDependency));
        }
    }
}
