using System.Xml;

namespace JetCode.Configuration
{
    public class ConfigurationManager
    {
        private readonly ClassFactoryConfiguration _classFactoryConfigure = null;
        private readonly XmlNode _configurationData = null;

        public ConfigurationManager(XmlNode sections)
        {
            this._configurationData = sections;
            this._classFactoryConfigure = new ClassFactoryConfiguration(this.GetData("ClassFactory"));
        }

        public ClassFactoryConfiguration ClassFactoryConfigure
        {
            get { return this._classFactoryConfigure; }
        }

        private XmlNode GetData(string key)
        {
            return this._configurationData.SelectSingleNode(key);
        }
    }
}