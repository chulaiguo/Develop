using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using JetCode.BizSchema;

namespace JetCode.FactoryWCF
{
    public static class Utils
    {
        public static SortedList<string, Type> GetTypeList(string projectName, string dllName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            StringCollection list = new StringCollection();
            list.Add(string.Format(@"D:\Work\{0}V2\Bin\{1}", projectName, dllName));
            list.Add(string.Format(@"D:\Work\{0}\Bin\{1}", projectName, dllName));

            foreach (string dll in list)
            {
                if (File.Exists(dll))
                {
                    Assembly assembly = Assembly.LoadFrom(dll);
                    if (assembly != null)
                    {
                        Type[] typeList = assembly.GetTypes();
                        foreach (Type item in typeList)
                        {
                            if (retList.ContainsKey(item.Name))
                                continue;

                            retList.Add(item.Name, item);
                        }
                    }
                }
            }

            return retList;
        }

        public static Type GetDataType(MappingSchema mappingSchema, ObjectSchema objectSchema)
        {
            Assembly assembly = GetDataAssembly(mappingSchema.Name);
            if (assembly == null)
                return null;

            Type[] types = assembly.GetTypes();
            foreach (Type item in types)
            {
                if (item.Name == objectSchema.Name + "Data")
                    return item;
            }

            return null;
        }

        public static Type GetDataViewType(MappingSchema mappingSchema, ObjectSchema objectSchema)
        {
            Assembly assembly = GetDataAssembly(mappingSchema.Name);
            if (assembly == null)
                return null;

            Type[] types = assembly.GetTypes();
            foreach (Type item in types)
            {
                if (item.Name == objectSchema.Name + "View")
                    return item;
            }

            return null;
        }

        private static Assembly GetDataAssembly(string projectName)
        {
            StringCollection list = new StringCollection();
            list.Add(string.Format(@"D:\Work\{0}V2\Bin\{0}.Data.dll", projectName));
            list.Add(string.Format(@"D:\Work\{0}\Bin\{0}.Data.dll", projectName));

            foreach (string item in list)
            {
                if (File.Exists(item))
                {
                    return Assembly.LoadFrom(item);
                }
            }

            return null;
        }
    }
}