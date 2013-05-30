namespace Needle.Configuration
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Needle.Exceptions;
    using Needle.Properties;

    public class NeedleConfiguration
    {
        public NeedleConfiguration()
        {
            this.Mappings = new List<MappingConfigurationElement>();
        }
        
        public IList<MappingConfigurationElement> Mappings { get; private set; }

        protected void ParseConfigurationElement(XDocument xmlDocument)
        {
            var mappingElements = xmlDocument.Descendants("mapping");
            foreach (var mappingElement in mappingElements)
            {
                this.Mappings.Add(this.ParseMappingElement(mappingElement));
            }
        }

        protected MappingConfigurationElement ParseMappingElement(XElement mappingElement)
        {
            if (mappingElement.Attribute("from") == null)  
            {
                throw new InvalidConfigurationElementException(Resources.FromTypeMissingFromConfigurationMapping);
            }

            if (mappingElement.Attribute("to") == null)
            {
                throw new InvalidConfigurationElementException(Resources.ToTypeMissingFromConfigurationMapping);
            }

            string fromType = mappingElement.Attribute("from").Value;
            string toType = mappingElement.Attribute("to").Value;
            string lifetime = "Transient";
            string id = string.Empty;
            if (mappingElement.Attribute("lifetime") != null)
            {
                lifetime = mappingElement.Attribute("lifetime").Value;
            }

            if (mappingElement.Attribute("id") != null)
            {
                id = mappingElement.Attribute("id").Value;
            }

            return new MappingConfigurationElement(fromType, toType, lifetime, id);
        }
    }
}
