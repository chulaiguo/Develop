using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryRemoting
{
    public class FactoryBizServiceWrapper : FactoryBase
    {
        public FactoryBizServiceWrapper(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.IRemotingService;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.{1}ServiceWrapper", base.ProjectName, Utils._ServiceName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.{1}Service.dll", base.ProjectName, Utils._ServiceName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Service") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService
                writer.WriteLine("\tpublic static class {0}Wrapper", className);
                writer.WriteLine("\t{");

                MethodInfo[] list = item.Value.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (MethodInfo info in list)
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityToken token)",
                       info.ReturnType.FullName, info.Name, this.GetInputParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn {0}({2}ServiceBuilder.GetFactory(), {1});", info.Name, this.GetInvokeParas(info), Utils._ServiceName);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();

                    if (className == "BizPrinting")
                    {
                        writer.WriteLine("\t\tpublic static {0} {1}({2} string _server_location_, SecurityToken token)",
                            info.ReturnType.FullName, info.Name, this.GetInputParas(info));
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\treturn {0}({2}ServiceBuilder.GetFactory(_server_location_), {1});",
                            info.Name, this.GetInvokeParas(info), Utils._ServiceName);
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }

                    writer.WriteLine("\t\tprivate static {0} {1}(I{3}ServiceFactory factory, {2} SecurityToken token)",
                        info.ReturnType.FullName, info.Name, this.GetInputParas(info), Utils._ServiceName);
                    writer.WriteLine("\t\t{");

                    ParameterInfo[] paraList = info.GetParameters();
                    writer.WriteLine("\t\t\tstring[] paraNames = new string[{0}];", paraList.Length);
                    writer.WriteLine("\t\t\tobject[] paraValues = new object[{0}];", paraList.Length);
                    for (int i = 0; i < paraList.Length; i++)
                    {
                        ParameterInfo para = paraList[i];
                        writer.WriteLine("\t\t\tparaNames[{0}] = \"{1}\";", i, para.Name);
                        writer.WriteLine("\t\t\tparaValues[{0}] = {1};", i, para.Name);
                    }
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tSecurityToken _token_ = SecurityToken.CreateFrameworkToken(token, paraNames, paraValues);");
                    writer.WriteLine();

                    writer.WriteLine("\t\t\tbyte[] _data_ = factory.Get{0}Result(\"{1}\", {2}ServiceBuilder.Serialize(_token_));", className, info.Name, Utils._ServiceName);
                    if (info.ReturnType.IsValueType)
                    {
                        writer.WriteLine("\t\t\t{0} _result_ =  ({0})Compression.DecompressToObject(_data_);", info.ReturnType.FullName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t{0} _result_ = null;", info.ReturnType.FullName);
                        writer.WriteLine("\t\t\tif(_data_ != null)");
                        writer.WriteLine("\t\t\t{");
                        if (info.ReturnType == typeof(byte[]))
                        {
                            writer.WriteLine("\t\t\t\t_result_ =  Compression.DecompressToByteArray(_data_) as {0};", info.ReturnType.FullName);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t_result_ =  Compression.DecompressToObject(_data_) as {0};", info.ReturnType.FullName);
                        }
                        writer.WriteLine("\t\t\t}");
                    }
                    writer.WriteLine("\t\t\treturn _result_;");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            this.WriteBasicServiceBuilder(writer);
        }

        private void WriteBasicServiceBuilder(StringWriter writer)
        {
            writer.WriteLine("\tinternal static class {0}ServiceBuilder", Utils._ServiceName);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tinternal static byte[] Serialize(object obj)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tnew System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, obj);");
            writer.WriteLine("\t\t\t\treturn stream.ToArray();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tinternal static I{0}ServiceFactory GetFactory()", Utils._ServiceName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn (I{1}ServiceFactory) Cheke.ClassFactory.ClassBuilder.GetFactory(\"{0}.{1}ServiceFactory\");", base.ProjectName, Utils._ServiceName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tinternal static I{0}ServiceFactory GetFactory(string location)", Utils._ServiceName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn Activator.GetObject(typeof(I{0}ServiceFactory), location) as I{0}ServiceFactory;", Utils._ServiceName);
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0} {1},", info.ParameterType.FullName, info.Name);
            }

            return builder.ToString();
        }

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0},", info.Name);
            }

            builder.AppendFormat("token");
            return builder.ToString();
        }
    }
}
