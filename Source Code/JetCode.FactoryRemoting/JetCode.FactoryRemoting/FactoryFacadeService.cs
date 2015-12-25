using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryRemoting
{
    public class FactoryFacadeService : FactoryBase
    {
        public FactoryFacadeService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.BizData;", base.ProjectName);
            writer.WriteLine("using {0}.FacadeService;", base.ProjectName);
            writer.WriteLine("using {0}.IRemotingService;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.RemotingService", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class FacadeServiceFactory: MarshalByRefObject, IFacadeServiceFactory");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.FacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Service") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService

                writer.WriteLine("\t\tpublic byte[] Get{0}Result(string actionName, byte[] paras)", className);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tSecurityToken token = this.DeserializeToken(paras);");
                writer.WriteLine("\t\t\tswitch (actionName)");
                writer.WriteLine("\t\t\t{");

                SortedList<string, List<MethodInfo>> index = this.GetMethods(item.Value);
                foreach (KeyValuePair<string, List<MethodInfo>> pair in index)
                {
                    writer.WriteLine("\t\t\t\tcase \"{0}\" :", pair.Key);
                    writer.WriteLine("\t\t\t\t{");
                    if (pair.Value.Count == 1)
                    {
                        MethodInfo method = pair.Value[0];
                        writer.WriteLine("\t\t\t\t\t{0} _result_  = {1}Wrapper.{2}({3});", method.ReturnType.FullName, className, pair.Key, this.GetInputParas(method));
                        if (method.ReturnType.IsValueType)
                        {
                            if (method.ReturnType == typeof(DateTime))
                            {
                                writer.WriteLine("\t\t\t\t\t_result_ = _result_.ToUniversalTime();");
                            }
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t\tif(_result_ == null)");
                            writer.WriteLine("\t\t\t\t\t\treturn null;");
                            writer.WriteLine();
                        }

                        writer.WriteLine("\t\t\t\t\treturn Compression.Compress(_result_);");
                    }
                    else
                    {
                        foreach (MethodInfo method in pair.Value)
                        {
                            writer.WriteLine("\t\t\t\t\tif(token.ParameterNames == \"{0}\")", this.GetParaNameList(method));
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\t{0} _result_  = {1}Wrapper.{2}({3});", method.ReturnType.FullName, className, pair.Key, this.GetInputParas(method));
                            if (method.ReturnType.IsValueType)
                            {
                                if (method.ReturnType == typeof(DateTime))
                                {
                                    writer.WriteLine("\t\t\t\t\t\t_result_ = _result_.ToUniversalTime();");
                                }
                            }
                            else
                            {
                                writer.WriteLine("\t\t\t\t\t\tif(_result_ == null)");
                                writer.WriteLine("\t\t\t\t\t\t\treturn null;");
                                writer.WriteLine();
                            }
                            writer.WriteLine("\t\t\t\t\t\treturn Compression.Compress(_result_);");
                            writer.WriteLine("\t\t\t\t\t}");
                        }

                        writer.WriteLine("\t\t\t\t\treturn null;");
                    }
                    writer.WriteLine("\t\t\t\t}");
                }
               
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\treturn null;");
                writer.WriteLine("\t\t}");
            }

            writer.WriteLine("\t\tprivate SecurityToken DeserializeToken(byte[] data)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream(data))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream) as SecurityToken;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private SortedList<string, List<MethodInfo>> GetMethods(Type type)
        {
            SortedList<string, List<MethodInfo>> retIndex = new SortedList<string, List<MethodInfo>>();

            MethodInfo[] list = type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (MethodInfo info in list)
            {
                if (!retIndex.ContainsKey(info.Name))
                {
                    retIndex.Add(info.Name, new List<MethodInfo>());
                }
               
                retIndex[info.Name].Add(info);
            }

            return retIndex;
        }

        private string GetParaNameList(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0}|", info.Name);
            }

            return builder.ToString().TrimEnd('|');
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            for (int i = 0; i < list.Length; i++)
            {
                ParameterInfo info = list[i];
                if (info.ParameterType == typeof(DateTime))
                {
                    builder.AppendFormat("new DateTime(((DateTime)token.GetParameter({0})).Ticks, DateTimeKind.Utc).ToLocalTime(),", i);
                }
                else
                {
                    builder.AppendFormat("({0})token.GetParameter({1}),", info.ParameterType.FullName, i);
                }
            }

            return string.Format("{0} token", builder);
        }
    }
}
