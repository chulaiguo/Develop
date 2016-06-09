using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryClientWCFServiceWrapper : FactoryBase
    {
        public FactoryClientWCFServiceWrapper(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Threading;");
            writer.WriteLine("using System.ServiceModel;");
            writer.WriteLine("using {0}.IWCFService;", base.ProjectName);
            writer.WriteLine("using {0}.DTO;", base.ProjectName);

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
            this.WriteBasicWrapper(writer);
            this.WriteFacadeWrapper(writer);
        }

        private void WriteBasicWrapper(StringWriter writer)
        {
            string dllName = string.Format("{0}.IDataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string typeKey = string.Format("I{0}DataService", item.Alias);
                if (!typeList.ContainsKey(typeKey))
                    continue;

                writer.WriteLine("\tpublic class {0}Wrapper", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate ChannelFactory<I{0}WCFService> _channelFactory = null;", item.Alias);
                writer.WriteLine("\t\tprivate I{0}WCFService _factory = null;", item.Alias);
                writer.WriteLine("\t\tprivate SynchronizationContext _uiContext = null;");
                writer.WriteLine("\t\tprivate event UpdateUIDelegate _updateUI;");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate {0}Wrapper()", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._uiContext = SynchronizationContext.Current;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._channelFactory = new ChannelFactory<I{0}WCFService>(\"{1}.WCFService.{0}WCFService\");", item.Alias, base.ProjectName);
                writer.WriteLine("\t\t\tthis._factory = this._channelFactory.CreateChannel();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void PostDataToUI(object state)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (this._updateUI == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._updateUI(state);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                //Save
                writer.WriteLine("\t\tpublic static void SaveItem({0}DataDTO entity, UpdateUIDelegate updateUI)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Wrapper wrapper = new {0}Wrapper();", item.Alias);
                writer.WriteLine("\t\t\twrapper._updateUI = updateUI;");
                writer.WriteLine("\t\t\twrapper._factory.BeginSaveItem(entity, Identity.Token, new AsyncCallback(wrapper.CallBack_SaveItem), null);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void CallBack_SaveItem(IAsyncResult syn)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tResultDTO obj = this._factory.EndSaveItem(syn);");
                writer.WriteLine("\t\t\tthis._channelFactory.Close();");
                writer.WriteLine();
                writer.WriteLine("\t\t\tif (this._uiContext != null)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._uiContext.Post(PostDataToUI, obj);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic static void SaveList({0}DataDTOCollection list, UpdateUIDelegate updateUI)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Wrapper wrapper = new {0}Wrapper();", item.Alias);
                writer.WriteLine("\t\t\twrapper._updateUI = updateUI;");
                writer.WriteLine("\t\t\twrapper._factory.BeginSaveList(list, Identity.Token, new AsyncCallback(wrapper.CallBack_SaveList), null);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void CallBack_SaveList(IAsyncResult syn)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tResultDTO obj = this._factory.EndSaveList(syn);");
                writer.WriteLine("\t\t\tthis._channelFactory.Close();");
                writer.WriteLine();
                writer.WriteLine("\t\t\tif (this._uiContext != null)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._uiContext.Post(PostDataToUI, obj);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                MethodInfo[] methodList = typeList[typeKey].GetMethods();
                SortedList<string, string> methodIndex = new SortedList<string, string>();
                methodIndex.Add("Save", "Save");
                foreach (MethodInfo method in methodList)
                {
                    if (methodIndex.ContainsKey(method.Name))
                        continue;

                    methodIndex.Add(method.Name, method.Name);

                    writer.WriteLine("\t\tpublic static void {0}({1})", method.Name, this.GetParas(method));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}Wrapper wrapper = new {0}Wrapper();", item.Alias);
                    writer.WriteLine("\t\t\twrapper._updateUI = updateUI;");
                    writer.WriteLine("\t\t\twrapper._factory.Begin{0}({1}, new AsyncCallback(wrapper.CallBack_{0}), null);", method.Name, this.GetInvokeParas(method));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();

                    writer.WriteLine("\t\tprivate void CallBack_{0}(IAsyncResult syn)", method.Name);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0} obj = this._factory.End{1}(syn);", this.GetDTOType(method.ReturnType.Name), method.Name);
                    writer.WriteLine("\t\t\tthis._channelFactory.Close();");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif (this._uiContext != null)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._uiContext.Post(PostDataToUI, obj);");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
            }
        }

        private void WriteFacadeWrapper(StringWriter writer)
        {
            string dllName = string.Format("{0}.IFacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key == "IFacadeServiceFactory" || pair.Key == "IBizExcelService")
                    continue;

                string bizName = pair.Key.Substring(1, pair.Key.Length - "Service".Length - 1);

                writer.WriteLine("\tpublic class {0}Wrapper", bizName);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate ChannelFactory<I{0}WCFService> _channelFactory = null;", bizName);
                writer.WriteLine("\t\tprivate I{0}WCFService _factory = null;", bizName);
                writer.WriteLine("\t\tprivate SynchronizationContext _uiContext = null;");
                writer.WriteLine("\t\tprivate event UpdateUIDelegate _updateUI;");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate {0}Wrapper()", bizName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._uiContext = SynchronizationContext.Current;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._channelFactory = new ChannelFactory<I{0}WCFService>(\"{1}.WCFService.{0}WCFService\");", bizName, base.ProjectName);
                writer.WriteLine("\t\t\tthis._factory = this._channelFactory.CreateChannel();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void PostDataToUI(object state)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (this._updateUI == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._updateUI(state);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                MethodInfo[] methodList = pair.Value.GetMethods();
                SortedList<string, string> methodIndex = new SortedList<string, string>();
                foreach (MethodInfo method in methodList)
                {
                    if (methodIndex.ContainsKey(method.Name))
                        continue;

                    methodIndex.Add(method.Name, method.Name);

                    writer.WriteLine("\t\tpublic static void {0}({1})", method.Name, this.GetParas(method));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}Wrapper wrapper = new {0}Wrapper();", bizName);
                    writer.WriteLine("\t\t\twrapper._updateUI = updateUI;");
                    writer.WriteLine("\t\t\twrapper._factory.Begin{0}({1}, new AsyncCallback(wrapper.CallBack_{0}), null);", method.Name, this.GetInvokeParas(method));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();

                    writer.WriteLine("\t\tprivate void CallBack_{0}(IAsyncResult syn)", method.Name);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0} obj = this._factory.End{1}(syn);", this.GetDTOType(method.ReturnType.Name), method.Name);
                    writer.WriteLine("\t\t\tthis._channelFactory.Close();");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif (this._uiContext != null)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._uiContext.Post(PostDataToUI, obj);");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
            }
        }

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0}, ", item.Name);
            }

            builder.AppendFormat("Identity.Token");
            return builder.ToString();
        }
   
        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0}", this.GetParameterType(item));
            }

            builder.AppendFormat("UpdateUIDelegate updateUI");
            return builder.ToString();
        }

        private string GetParameterType(ParameterInfo item)
        {
            string returnTypeName = item.ParameterType.Name;
            if (item.ParameterType.IsClass)
            {
                string dtoType = this.GetDTOType(returnTypeName);
                return string.Format("{0} {1}, ", dtoType, item.Name);
            }

            return string.Format("{0} {1}, ", returnTypeName, item.Name);
        }

        private string GetDTOType(string originalTypeName)
        {
            if (originalTypeName == "Result")
            {
                return "ResultDTO";
            }

            if (originalTypeName.EndsWith("DataCollection"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "DataCollection".Length);
                return string.Format("{0}DataDTOCollection", objName);
            }

            if (originalTypeName.EndsWith("ViewCollection"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "ViewCollection".Length);
                return string.Format("{0}ViewDTOCollection", objName);

            }

            if (originalTypeName.EndsWith("Data"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "Data".Length);
                return string.Format("{0}DataDTO", objName);

            }

            if (originalTypeName.EndsWith("View"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "View".Length);
                return string.Format("{0}ViewDTO", objName);
            }

            if (originalTypeName.StartsWith("Biz"))
            {
                if(originalTypeName.EndsWith("Collection"))
                {
                    string objName = originalTypeName.Substring(0, originalTypeName.Length - "Collection".Length);
                    return string.Format("{0}DTOCollection", objName);
                    
                }
                else
                {
                    string objName = originalTypeName;
                    return string.Format("{0}DTO", objName);
                }
            }

            return originalTypeName;
        }
    }
}
