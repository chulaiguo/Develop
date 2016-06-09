using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryViewObj
{
    public class FactoryBizService : FactoryBase
    {
        public FactoryBizService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.DTO;", base.ProjectName);
            writer.WriteLine("using {0}.WebAPIJsonWrapper;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewModel", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            SortedList<string, Type> typeList = Utils.GetBizServiceTypeList(base.ProjectName);
            writer.WriteLine("\tpublic static class Biz{0}", Utils._ServiceName);
            writer.WriteLine("\t{");
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Service") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "Service".Length);//BizLoginService

                MethodInfo[] list = item.Value.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (MethodInfo info in list)
                {
                    if(!this.IsReturnTypeValid(info))
                        continue;

                    this.CreateMethodWithToken(writer, info, className);
                    writer.WriteLine();
                }
            }
            writer.WriteLine("\t}");
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

            if (info.ReturnType.Name.EndsWith("Collection"))
            {
                if (info.ReturnType.Name.EndsWith("DataCollection"))
                {
                    string typeName = info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "DataCollection".Length);
                    writer.WriteLine("\t\tpublic static {0}[] {1}({2})", typeName, info.Name, inputPara);
                }
                else
                {
                    string typeName = info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Collection".Length);
                    writer.WriteLine("\t\tpublic static {0}[] {1}({2})", typeName, info.Name, inputPara);
                }
            }
            else
            {
                if (info.ReturnType.Name.EndsWith("Data"))
                {
                    string typeName = info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Data".Length);
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", typeName, info.Name, inputPara);
                }
                else
                {
                    string typeName = info.ReturnType.Name;
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", typeName, info.Name, inputPara);
                }
            }

            //body
            writer.WriteLine("\t\t{");

            //convert paras
            ParameterInfo[] paraList = info.GetParameters();
            foreach (ParameterInfo para in paraList)
            {
                if (para.ParameterType.Name.EndsWith("Collection"))
                {
                    if (para.ParameterType.Name.EndsWith("DataCollection"))
                    {
                        string paraType = para.ParameterType.Name.Substring(0, para.ParameterType.Name.Length - "DataCollection".Length);
                        writer.WriteLine("\t\t\t{0}DataDTO[] _{1}_dto_ = {0}.Serialize({1});", paraType, para.Name);
                    }
                    else
                    {
                        string paraType = para.ParameterType.Name.Substring(0, para.ParameterType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\t\t{0}DTO[] _{1}_dto_ = {0}.Serialize({1});", paraType, para.Name);
                    }
                }
                else
                {
                    if (para.ParameterType.Name.EndsWith("Data") || para.ParameterType.Name.EndsWith("View")
                        || para.ParameterType.Name.StartsWith("Biz"))
                    {
                        string paraType = para.ParameterType.Name;
                        writer.WriteLine("\t\t\t{0}DTO _{1}_dto_ = {1}.Serialize();", paraType, para.Name);
                    }
                    else
                    {
                        string paraType = para.ParameterType.Name;
                        writer.WriteLine("\t\t\t{0} _{1}_dto_ = {1};", paraType, para.Name);
                    }
                }
            }
            writer.WriteLine("\t\t\tSecurityTokenDTO _token_dto_ = token.Serialize();");
            writer.WriteLine();

            //invoke
            if (info.ReturnType.Name.EndsWith("Collection"))
            {
                if (info.ReturnType == typeof(StringCollection))
                {
                    writer.WriteLine("\t\t\treturn {0}Wrapper.{1}({2} _token_dto_);", className, info.Name, this.GetInvokeParas(info));
                }
                else
                {
                    string paraType = info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Collection".Length);

                    writer.WriteLine("\t\t\t{0}DTO[] _result_ = {1}Wrapper.{2}({3} _token_dto_);", paraType, className, info.Name, this.GetInvokeParas(info));
                    writer.WriteLine("\t\t\tif(_result_ == null)");
                    writer.WriteLine("\t\t\t\treturn null;");
                    writer.WriteLine();

                    if (info.ReturnType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\treturn {0}.Deserialize(_result_);",
                            info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "DataCollection".Length));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\treturn {0}.Deserialize(_result_);",
                           info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Collection".Length));
                    }
                }
            }
            else
            {
                if (info.ReturnType.Name.EndsWith("Data") || info.ReturnType.Name.EndsWith("View")
                    || info.ReturnType.Name.StartsWith("Biz") || info.ReturnType.Name == "Result")
                {
                    if (info.ReturnType.Name.EndsWith("Data"))
                    {
                        string paraType = info.ReturnType.Name;

                        writer.WriteLine("\t\t\t{0}DTO _result_ = {1}Wrapper.{2}({3} _token_dto_);", paraType, className, info.Name, this.GetInvokeParas(info));
                        writer.WriteLine("\t\t\tif(_result_ == null)");
                        writer.WriteLine("\t\t\t\treturn null;");
                        writer.WriteLine();

                        writer.WriteLine("\t\t\treturn new {0}(_result_);",
                         info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - "Data".Length));
                    }
                    else
                    {
                        string paraType = info.ReturnType.Name;

                        writer.WriteLine("\t\t\t{0}DTO _result_ = {1}Wrapper.{2}({3} _token_dto_);", paraType, className, info.Name, this.GetInvokeParas(info));
                        writer.WriteLine("\t\t\tif(_result_ == null)");
                        writer.WriteLine("\t\t\t\treturn null;");
                        writer.WriteLine();

                        writer.WriteLine("\t\t\treturn new {0}(_result_);", info.ReturnType.Name);
                    }
                }
                else
                {
                    writer.WriteLine("\t\t\treturn {0}Wrapper.{1}({2} _token_dto_);", className, info.Name, this.GetInvokeParas(info));
                    writer.WriteLine();
                }
            }

            writer.WriteLine("\t\t}");
        }

        private bool IsReturnTypeValid(MethodInfo info)
        {
            if (info.ReturnType.IsValueType || info.ReturnType == typeof(string)
                || info.ReturnType.Name.EndsWith("[]")
                || info.ReturnType.Name == "Result"
                || info.ReturnType.Name.StartsWith("Biz")
                || info.ReturnType.Name.EndsWith("Data")
                || info.ReturnType.Name.EndsWith("View")
                || info.ReturnType.Name.EndsWith("Collection"))
                return true;

            return false;
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                if (info.ParameterType.Name.EndsWith("Collection"))
                {
                    if (info.ParameterType.Name.EndsWith("DataCollection"))
                    {
                        string paraType = info.ParameterType.Name.Substring(0, info.ParameterType.Name.Length - "DataCollection".Length);
                        builder.AppendFormat("{0}[] {1},", paraType, info.Name);
                    }
                    else
                    {
                        string paraType = info.ParameterType.Name.Substring(0, info.ParameterType.Name.Length - "Collection".Length);
                        builder.AppendFormat("{0}[] {1},", paraType, info.Name);
                    }
                }
                else
                {
                    if (info.ParameterType.Name.EndsWith("Data"))
                    {
                        string paraType = info.ParameterType.Name.Substring(0, info.ParameterType.Name.Length - "Data".Length);
                        builder.AppendFormat("{0} {1},", paraType, info.Name);
                    }
                    else
                    {
                        builder.AppendFormat("{0} {1},", info.ParameterType.Name, info.Name);
                    }
                }
                
            }

            return builder.ToString().TrimEnd(',');
        }

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("_{0}_dto_,", info.Name);
            }

            return builder.ToString();
        }
    }
}
