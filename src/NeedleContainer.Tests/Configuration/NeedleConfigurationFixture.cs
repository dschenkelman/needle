namespace Needle.Tests.Configuration
{
    using System.Linq;
    using System.Xml.Linq;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Needle.Configuration;
    using Needle.Container;
    
    [TestClass]
    public class NeedleConfigurationFixture
    {
        [TestMethod]
        public void ShouldParseMappingConfigurationFromXElement()
        {
            TestableNeedleConfiguration configuration = new TestableNeedleConfiguration();
            string xml = @"<needle>
                <mapping from='System.Int32' to='System.String'/>
                <mapping from='System.Int32' to='System.String' lifetime='singleton'/>
                <mapping from='System.Int32' to='System.String' id='customRegistration'/>
                <mapping from='System.Int32' to='System.String' id='anotherRegistration' lifetime='singleton'/>
              </needle>";

            XDocument doc = XDocument.Parse(xml);
            
            configuration.InvokeParseConfigurationElement(doc);

            Assert.AreEqual(4, configuration.Mappings.Count());
            var mappings = configuration.Mappings.ToList();
            mappings.ForEach(e => Assert.AreEqual(typeof(int), e.FromType));
            mappings.ForEach(e => Assert.AreEqual(typeof(string), e.ToType));
            
            // first mapping
            Assert.AreEqual(string.Empty, mappings[0].RegistrationId);
            Assert.AreEqual(RegistrationLifetime.Transient, mappings[0].Lifetime);

            // second mapping
            Assert.AreEqual(string.Empty, mappings[1].RegistrationId);
            Assert.AreEqual(RegistrationLifetime.Singleton, mappings[1].Lifetime);

            // third mapping
            Assert.AreEqual("customRegistration", mappings[2].RegistrationId);
            Assert.AreEqual(RegistrationLifetime.Transient, mappings[2].Lifetime);

            // fourth mapping
            Assert.AreEqual("anotherRegistration", mappings[3].RegistrationId);
            Assert.AreEqual(RegistrationLifetime.Singleton, mappings[3].Lifetime);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidConfigurationElementException))]
        public void ShouldThrowExceptionIfFromAttributeDoesNotExist()
        {
            XElement element = XElement.Parse("<mapping to='System.String'/>");
            TestableNeedleConfiguration configuration = new TestableNeedleConfiguration();
            configuration.InvokeParseMappingElement(element);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidConfigurationElementException))]
        public void ShouldThrowExceptionIfToAttributeDoesNotExist()
        {
            XElement element = XElement.Parse("<mapping from='System.String'/>");
            TestableNeedleConfiguration configuration = new TestableNeedleConfiguration();
            configuration.InvokeParseMappingElement(element);
        }

        public class TestableNeedleConfiguration : NeedleConfiguration
        {
            public void InvokeParseConfigurationElement(XDocument doc)
            {
                this.ParseConfigurationElement(doc);
            }

            public void InvokeParseMappingElement(XElement element)
            {
                this.ParseMappingElement(element);
            }
        }
    }
}
