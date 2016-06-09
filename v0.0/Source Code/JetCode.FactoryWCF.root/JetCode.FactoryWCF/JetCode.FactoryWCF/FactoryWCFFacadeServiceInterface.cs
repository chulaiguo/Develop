using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFFacadeServiceInterface : FactoryBase
    {
        public FactoryWCFFacadeServiceInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.ServiceModel;");
            writer.WriteLine("using {0}.DTO;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IWCFService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.IFacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (item.Key == "IFacadeServiceFactory" || item.Key == "IBizExcelService")
                    continue;

                string objName = item.Key.Substring(1, item.Key.Length - "Service".Length - 1);

                writer.WriteLine("\t[ServiceContract]");
                writer.WriteLine("\tpublic interface I{0}WCFService", objName);
                writer.WriteLine("\t{");
                this.WriteMethod(writer, item.Value);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteMethod(StringWriter writer, Type type)
        {
            SortedList<string, int> methodIndex = new SortedList<string, int>();

            MethodInfo[] methodList = type.GetMethods();
            writer.WriteLine("#if SILVERLIGHT");
            writer.WriteLine();

            foreach (MethodInfo item in methodList)
            {
                if(!methodIndex.ContainsKey(item.Name))
                {
                    methodIndex.Add(item.Name, 1);
                }
                else
                {
                    methodIndex[item.Name] += 1;
                    continue;//Not support overloading
                }

                string paras = this.GetParas(item);
                if (item.ReturnType.Name.EndsWith("DataCollection"))
                {
                    string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "DataCollection".Length);

                    //OperationContractAttribute
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true, Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true)]");
                    }
                    writer.WriteLine("\t\tIAsyncResult Begin{0}({1}, AsyncCallback callback, object asyncState);", item.Name, paras);
                    writer.WriteLine("\t\t{0}DataDTOCollection End{1}(System.IAsyncResult result);", objName, item.Name);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("Data"))
                {
                    string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "Data".Length);

                    //OperationContractAttribute
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true, Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true)]");
                    }
                    writer.WriteLine("\t\tIAsyncResult Begin{0}({1}, AsyncCallback callback, object asyncState);", item.Name, paras);
                    writer.WriteLine("\t\t{0}DataDTO End{1}(System.IAsyncResult result);", objName, item.Name);
                    writer.WriteLine();
                    continue;
                }    
                
                if (item.ReturnType.Name.EndsWith("ViewCollection"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "ViewCollection".Length);

                    //OperationContractAttribute
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true, Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true)]");
                    }
                    writer.WriteLine("\t\tIAsyncResult Begin{0}({1}, AsyncCallback callback, object asyncState);", item.Name, paras);
                    writer.WriteLine("\t\t{0}ViewDTOCollection End{1}(System.IAsyncResult result);", viewObj, item.Name);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("View"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "View".Length);

                    //OperationContractAttribute
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true, Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true)]");
                    }
                    writer.WriteLine("\t\tIAsyncResult Begin{0}({1}, AsyncCallback callback, object asyncState);", item.Name, paras);
                    writer.WriteLine("\t\t{0}ViewDTO End{1}(System.IAsyncResult result);", viewObj, item.Name);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.StartsWith("Biz"))
                {
                    //OperationContractAttribute
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true, Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true)]");
                    }
                    writer.WriteLine("\t\tIAsyncResult Begin{0}({1}, AsyncCallback callback, object asyncState);", item.Name, paras);
                    if (item.ReturnType.Name.EndsWith("Collection"))
                    {
                        string bizObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\t{0}DTOCollection End{1}(System.IAsyncResult result);", bizObj, item.Name);
                    }
                    else
                    {
                        writer.WriteLine("\t\t{0}DTO End{1}(System.IAsyncResult result);", item.ReturnType.Name, item.Name);  
                    }
                    writer.WriteLine();
                    continue;
                }


                string returnTypeName;
                if (item.ReturnType.Name == "Result")
                {
                    returnTypeName = "ResultDTO";
                }
                else
                {
                    returnTypeName = item.ReturnType.Name;
                }

                //OperationContractAttribute
                if (methodIndex[item.Name] > 1)//overloading
                {
                    writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true, Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                }
                else
                {
                    writer.WriteLine("\t\t[OperationContractAttribute(AsyncPattern = true)]");
                }
                writer.WriteLine("\t\tIAsyncResult Begin{0}({1}, AsyncCallback callback, object asyncState);", item.Name, paras);
                writer.WriteLine("\t\t{0} End{1}(System.IAsyncResult result);", returnTypeName, item.Name);
                writer.WriteLine(); 
            }

            writer.WriteLine("#else");
            writer.WriteLine();

            methodIndex.Clear();
            foreach (MethodInfo item in methodList)
            {
                if (!methodIndex.ContainsKey(item.Name))
                {
                    methodIndex.Add(item.Name, 1);
                }
                else
                {
                    methodIndex[item.Name] += 1;
                    continue;//Not support overloading
                }

                string paras = this.GetParas(item);
                if (item.ReturnType.Name.EndsWith("DataCollection"))
                {
                    string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "DataCollection".Length);

                    //OperationContract
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContract(Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                    }
                    
                    writer.WriteLine("\t\t{0}DataDTOCollection {1}({2});", objName, item.Name, paras);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("Data"))
                {
                    string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "Data".Length);

                    //OperationContract
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContract(Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                    } 
                    writer.WriteLine("\t\t{0}DataDTO {1}({2});", objName, item.Name, paras);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("ViewCollection"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "ViewCollection".Length);
                   
                    //OperationContract
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContract(Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                    }
                    writer.WriteLine("\t\t{0}ViewDTOCollection {1}({2});", viewObj, item.Name, paras);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("View"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "View".Length);

                    //OperationContract
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContract(Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                    }
                    writer.WriteLine("\t\t{0}ViewDTO {1}({2});", viewObj, item.Name, paras);
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.StartsWith("Biz"))
                {
                    //OperationContract
                    if (methodIndex[item.Name] > 1)//overloading
                    {
                        writer.WriteLine("\t\t[OperationContract(Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                    }

                    if (item.ReturnType.Name.EndsWith("Collection"))
                    {
                        string bizObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\t{0}DTOCollection {1}({2});", bizObj, item.Name, paras);
                    }
                    else
                    {
                        writer.WriteLine("\t\t{0}DTO {1}({2});", item.ReturnType.Name, item.Name, paras);
                    }
                    
                    writer.WriteLine();
                    continue;
                }

                string returnTypeName;
                if(item.ReturnType.Name == "Result")
                {
                    returnTypeName = "ResultDTO";
                }
                else
                {
                    returnTypeName = item.ReturnType.Name;
                }

                //OperationContract
                if (methodIndex[item.Name] > 1)//overloading
                {
                    writer.WriteLine("\t\t[OperationContract(Name=\"{0}_{1}\")]", item.Name, methodIndex[item.Name]);
                }
                else
                {
                    writer.WriteLine("\t\t[OperationContract]");
                }
                writer.WriteLine("\t\t{0} {1}({2});", returnTypeName, item.Name, paras);
                writer.WriteLine();
            }
            writer.WriteLine("#endif");
        }

        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0}", this.GetParameterType(item));
            }

            builder.AppendFormat("TokenDTO token");
            return builder.ToString();
        }

        private string GetParameterType(ParameterInfo item)
        {
            string returnTypeName = item.ParameterType.Name;
            if (item.ParameterType.IsClass)
            {
                if (returnTypeName.EndsWith("DataCollection"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "DataCollection".Length);
                    return string.Format("{0}DataDTOCollection {1}DTO, ", objName, item.Name);
                }

                if (returnTypeName.EndsWith("ViewCollection"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "ViewCollection".Length);
                    return string.Format("{0}ViewDTOCollection {1}DTO, ", objName, item.Name);

                }

                if (returnTypeName.EndsWith("Data"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "Data".Length);
                    return string.Format("{0}DataDTO {1}DTO, ", objName, item.Name);

                }

                if (returnTypeName.EndsWith("View"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "View".Length);
                    return string.Format("{0}ViewDTO {1}DTO, ", objName, item.Name);
                }

                if (returnTypeName.StartsWith("Biz"))
                {
                    string objName = returnTypeName;
                    return string.Format("{0}DTO {1}DTO, ", objName, item.Name);
                }
            }

            return string.Format("{0} {1}, ", returnTypeName, item.Name);
        }
    }
}
