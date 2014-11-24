using System.Xml;

namespace JetCode.Configuration
{
    public class ClassFactoryConfiguration
    {
        private readonly XmlNode _xmlClassFactory = null;

        public ClassFactoryConfiguration(XmlNode configData)
        {
            this._xmlClassFactory = configData;
        }

        public string GetFactoryLocation(string name)
        {
            XmlNode node = this._xmlClassFactory.SelectSingleNode("Class[@name='" + name + "']");
            if (node.Attributes["location"] != null)
            {
                return node.Attributes["location"].Value;
            }
            return string.Empty;
        }

        public string GetFactoryType(string name)
        {
            return this._xmlClassFactory.SelectSingleNode("Class[@name='" + name + "']").Attributes["type"].Value;
        }
    }
}