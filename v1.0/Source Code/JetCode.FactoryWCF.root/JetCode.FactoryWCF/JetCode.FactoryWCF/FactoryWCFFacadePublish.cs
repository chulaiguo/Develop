using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFFacadePublish : FactoryBase
    {
        private readonly SortedList<string, string> _list = new SortedList<string, string>();
        public FactoryWCFFacadePublish(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        public SortedList<string, string> List
        {
            get { return _list; }
        }


        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
        }

        protected override void EndWrite(StringWriter writer)
        {
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
                string publisher = string.Format("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"{0}.WCFService.{1}WCFService\" %>", base.ProjectName, objName);
                this.List.Add(objName, publisher);
                writer.WriteLine(publisher);
            }
        }
    }
}
