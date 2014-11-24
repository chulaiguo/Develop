using System;
using System.Configuration;
using System.Xml;

namespace JetCode.Configuration
{
    public class ConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Type type = Type.GetType(section.Attributes["type"].Value);
            
            try
            {
                return Activator.CreateInstance(type, new object[] { section });
            }
            catch
            {
                return null;
            }
        }
    }


}
