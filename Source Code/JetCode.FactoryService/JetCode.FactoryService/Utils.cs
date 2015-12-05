using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace JetCode.FactoryService
{
    public static class Utils
    {
        public const string _ServiceName = "MAS";
         
        public static SortedList<string, Type> GetTypeList(string projectName, string dllName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            StringCollection list = new StringCollection();
            list.Add(string.Format(@"E:\Work\{0}\Bin\{1}", projectName, dllName));
            list.Add(string.Format(@"E:\Work\{0}V2\Bin\{1}", projectName, dllName));
            
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
    }
}
