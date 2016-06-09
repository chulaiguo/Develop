using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryFacadeService
{
    public class FactoryFacadeServiceWrapper : FactoryBase
    {
        public FactoryFacadeServiceWrapper(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.BizData;", base.ProjectName);
            writer.WriteLine("using {0}.FacadeService;", base.ProjectName);


            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.FacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.StartsWith("Biz") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "Service".Length);//BizLoginService
                writer.WriteLine("\tpublic static class {0}Wrapper", className);
                writer.WriteLine("\t{");

                //self class
                MethodInfo[] methods = item.Value.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                foreach (MethodInfo info in methods)
                {
                    if (info.IsVirtual)
                        continue;

                    WriteMethod(writer, info, className);
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

        }

        private void WriteMethod(StringWriter writer, MethodInfo info, string className)
        {
            string returnType = info.ReturnType.ToString();
            if (info.ReturnType == typeof (void))
            {
                returnType = "void";
            }

            writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityToken token)", returnType, info.Name, this.GetDeclairParas(info));
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Service svr = new {0}Service(token);", className);
            writer.WriteLine("\t\t\t{0}svr.{1}({2});", returnType == "void" ? "" : "return ", info.Name, this.GetInputParas(info));
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private string GetDeclairParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0} {1},", item.ParameterType.FullName, item.Name);
            }

            return builder.ToString();
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0},", info.Name);
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}
