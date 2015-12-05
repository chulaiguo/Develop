using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryService
{
    public class FactoryViewObj : FactoryBase
    {
        public FactoryViewObj(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.{1}ServiceWrapper;", base.ProjectName, Utils._ServiceName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewObj", base.ProjectName);
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
                if (!item.Key.StartsWith("Biz"))
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService
                writer.WriteLine("\tpublic static class {0}", className);
                writer.WriteLine("\t{");

                MethodInfo[] list = item.Value.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (MethodInfo info in list)
                {
                    this.CreateMethod(writer, info);
                    writer.WriteLine();
                    this.CreateMethodWithToken(writer, info, className);
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void CreateMethod(StringWriter writer, MethodInfo info)
        {
            //header
            if (info.ReturnType.Name.EndsWith("Data"))
            {
                writer.WriteLine("\t\tpublic static {0} {1}({2})",
                   info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Data".Length),
                   info.Name, this.GetInputParas(info));
            }
            else if (info.ReturnType.Name.EndsWith("DataCollection"))
            {
                writer.WriteLine("\t\tpublic static {0}Collection {1}({2})",
                   info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "DataCollection".Length),
                   info.Name, this.GetInputParas(info));
            }
            else
            {
               writer.WriteLine("\t\tpublic static {0} {1}({2})",
                    info.ReturnType.FullName, info.Name, this.GetInputParas(info));
            }

            //body
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn {0}({1} Identity.Token);", info.Name, this.GetInvokeParas(info));
            writer.WriteLine("\t\t}");
          
        }

        private void CreateMethodWithToken(StringWriter writer, MethodInfo info, string className)
        {
            //header
            string inputPara = this.GetInputParas(info);
            if (string.IsNullOrEmpty(inputPara))
            {
                inputPara = "SecurityToken token";
            }
            else
            {
                inputPara += ", SecurityToken token";
            }

            if (info.ReturnType.Name.EndsWith("Data"))
            {
                writer.WriteLine("\t\tpublic static {0} {1}({2})",
                   info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Data".Length), info.Name, inputPara);
            }
            else if (info.ReturnType.Name.EndsWith("DataCollection"))
            {
                writer.WriteLine("\t\tpublic static {0}Collection {1}({2})",
                   info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "DataCollection".Length), info.Name, inputPara);
            }
            else
            {
                writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType.FullName, info.Name, inputPara);
            }

            //body
            writer.WriteLine("\t\t{");

            ParameterInfo[] paraList = info.GetParameters();
            foreach (ParameterInfo para in paraList)
            {
                if (!para.ParameterType.IsValueType && para.ParameterType.Name != "EmailMessageData")
                {
                    if (para.ParameterType.Name.EndsWith("Data") ||
                        para.ParameterType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tif({0} != null)", para.Name);
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\t{0} = new {1}({0});", para.Name, para.ParameterType.FullName);
                        writer.WriteLine("\t\t\t}");
                    }
                }
            }
            writer.WriteLine();

            writer.WriteLine("\t\t\t{0} _result_ = {1}Wrapper.{2}({3} token);",
                   info.ReturnType.FullName, className, info.Name, this.GetInvokeParas(info));
            if (info.ReturnType.IsValueType)
            {
                writer.WriteLine("\t\t\treturn _result_;");
            }
            else
            {
                writer.WriteLine("\t\t\tif(_result_ == null)");
                writer.WriteLine("\t\t\t\treturn null;");
                writer.WriteLine();

                if (info.ReturnType.Name.EndsWith("Data"))
                {
                    writer.WriteLine("\t\t\treturn new {0}(_result_);",
                        info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Data".Length));
                }
                else if (info.ReturnType.Name.EndsWith("DataCollection"))
                {
                    writer.WriteLine("\t\t\treturn new {0}Collection(_result_);",
                        info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "DataCollection".Length));
                }
                else
                {
                    writer.WriteLine("\t\t\treturn _result_;");
                }
            }

            writer.WriteLine("\t\t}");
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0} {1},", info.ParameterType.FullName, info.Name);
            }

            return builder.ToString().TrimEnd(',');
        }

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0},", info.Name);
            }

            return builder.ToString();
        }
    }
}
