using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JetCode.FactoryWebAPI
{
    public static class Utils
    {
        //public const string _ServiceName = "MAS";
        //public const string _ServiceName = "AS400";
        public const string _ServiceName = "LocalFile";

        private static float GetLastestVersion(string projectName)
        {
            DirectoryInfo info = new DirectoryInfo(string.Format(@"E:\Work\Projects\{0}", projectName));
            DirectoryInfo[] list = info.GetDirectories();

            float maxVersion = 0;
            foreach (DirectoryInfo item in list)
            {
                float version;
                if (float.TryParse(item.Name.Substring(1), out version))
                {
                    if (version >= maxVersion)
                    {
                        maxVersion = version;
                    }
                }
            }

            return maxVersion;
        }

        public static SortedList<string, Type> GetDataTypeList(string projectName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            string path = string.Format(@"E:\Work\Projects\{0}\v{1:f1}\Bin\{0}.Data.v{1:f1}.dll", projectName, GetLastestVersion(projectName));
            Assembly assembly = Assembly.LoadFrom(path);
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

            return retList;
        }

        public static SortedList<string, Type> GetBizDataTypeList(string projectName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            string path = string.Format(@"E:\Work\Projects\{0}\v{1:f1}\Bin\{0}.BizData.v{1:f1}.dll", projectName, GetLastestVersion(projectName));
            Assembly assembly = Assembly.LoadFrom(path);
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

            return retList;
        }

        public static SortedList<string, Type> GetDataServiceTypeList(string projectName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            string path = string.Format(@"E:\Work\Projects\{0}\v{1:f1}\Bin\{0}.DataService.v{1:f1}.dll", projectName, GetLastestVersion(projectName));
            Assembly assembly = Assembly.LoadFrom(path);
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

            return retList;
        }

        public static SortedList<string, Type> GetFacadeServiceTypeList(string projectName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            string path = string.Format(@"E:\Work\Projects\{0}\v{1:f1}\Bin\{0}.FacadeService.v{1:f1}.dll", projectName, GetLastestVersion(projectName));
            Assembly assembly = Assembly.LoadFrom(path);
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

            return retList;
        }

        public static SortedList<string, Type> GetBizServiceTypeList(string projectName)
        {
            SortedList<string, Type> retList = new SortedList<string, Type>();

            string path = string.Format(@"E:\Work\Projects\{0}\v{1:f1}\Bin\{0}.{2}Service.v{1:f1}.dll", projectName, GetLastestVersion(projectName), _ServiceName);
            Assembly assembly = Assembly.LoadFrom(path);
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

            return retList;
        }

    }
}
