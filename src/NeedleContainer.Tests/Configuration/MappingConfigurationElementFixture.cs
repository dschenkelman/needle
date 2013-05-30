namespace Needle.Tests.Configuration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Needle.Configuration;
    using Needle.Container;

    [TestClass]
    public class MappingConfigurationElementFixture
    {
        [TestMethod] 
        public void ShouldConvertFromTypeStringIntoTypeObject()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName);

            Assert.AreEqual(typeof(string), mappingConfigurationElement.FromType);
        }

        [TestMethod]
        public void ShouldConvertToTypeStringIntoTypeObject()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName);

            Assert.AreEqual(typeof(int), mappingConfigurationElement.ToType);
        }

        [TestMethod]
        public void ShouldConvertTransientStringIntoTransientLifetimeEnumValue()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";
            const string LifeTime = "Transient";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName, LifeTime, string.Empty);

            Assert.AreEqual(RegistrationLifetime.Transient, mappingConfigurationElement.Lifetime);
        }

        [TestMethod]
        public void ShouldConvertSingletonStringIntoSingletonLifetimeEnumValue()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";
            const string LifeTime = "Singleton";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName, LifeTime, string.Empty);

            Assert.AreEqual(RegistrationLifetime.Singleton, mappingConfigurationElement.Lifetime);
        }

        [TestMethod]
        public void ShouldUseTransientLifetimeAsDefault()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName);

            Assert.AreEqual(RegistrationLifetime.Transient, mappingConfigurationElement.Lifetime);
        }

        [TestMethod]
        public void ShouldEmptyIdAsDefaultRegistrationId()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName);

            Assert.AreEqual(string.Empty, mappingConfigurationElement.RegistrationId);
        }

        [TestMethod]
        public void ShouldUseIdAsRegistrationId()
        {
            const string FromTypeName = "System.String, mscorlib";
            const string ToTypeName = "System.Int32, mscorlib";
            const string Id = "Registration";
            const string LifeTime = "Singleton";

            var mappingConfigurationElement = new MappingConfigurationElement(FromTypeName, ToTypeName, LifeTime, Id);

            Assert.AreEqual("Registration", mappingConfigurationElement.RegistrationId);
        }
    }
}
