using System;

namespace JetCode.Configuration
{
    public static class ClassFactory
    {
        public static object GetFactory(string factoryName)
        {
            ClassFactoryConfiguration config = Manager.ClassFactoryConfigure;
            Type type = Type.GetType(config.GetFactoryType(factoryName));
            if (config.GetFactoryLocation(factoryName).Length != 0)
            {
                string factoryLocation = config.GetFactoryLocation(factoryName);
                return Activator.GetObject(type, factoryLocation);
            }

            return Activator.CreateInstance(type, null);
        }

        private static ConfigurationManager Manager
        {
            get { return (ConfigurationManager) System.Configuration.ConfigurationManager.GetSection("Framework"); }
        }
    }
}
