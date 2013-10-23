namespace Needle.Tests.Helpers
{
    using System;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Needle.Container;
    using Needle.Helpers;

    [TestClass]
    public class FactoryRegistryFixture
    {
        [TestMethod]
        public void ShouldHaveFactoryMappingAndBeAbleToGetItAfterAddingIt()
        {
            FactoryRegistry factoryRegistry = this.CreateFactoryRegistry();

            Factory<object> factory = () => new object();
            TypeMapping typeMapping = new TypeMapping(
                string.Empty, 
                typeof(uint), 
                typeof(int),
                RegistrationLifetime.Transient);

            Assert.IsFalse(factoryRegistry.HasFactory(typeMapping));

            factoryRegistry.AddFactory(typeMapping, factory);

            Assert.IsTrue(factoryRegistry.HasFactory(typeMapping));
            Assert.AreSame(factory, factoryRegistry.GetFactory(typeMapping));
        }

        [TestMethod]
        [ExpectedException(typeof(FactoryNotFoundException))]
        public void ShouldThrowWhenGettingFactoryForMappingThatWasNotAdded()
        {
            FactoryRegistry factoryRegistry = this.CreateFactoryRegistry();

            TypeMapping typeMapping = new TypeMapping(string.Empty, typeof(uint), typeof(int), RegistrationLifetime.Transient);

            factoryRegistry.GetFactory(typeMapping);
        }

        private FactoryRegistry CreateFactoryRegistry()
        {
            return new FactoryRegistry();
        }
    }
}
