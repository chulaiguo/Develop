using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryGetByMapParentInterface : FactoryBase
    {
        public FactoryGetByMapParentInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IDataService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                List<FieldSchema> pkList = obj.GetPKList();
                if(pkList.Count != 1)
                    continue;

                List<ObjectSchema> list = GetMapParents(obj);
                if(list.Count == 0)
                    continue;

                writer.WriteLine("\tpublic partial interface I{0}DataService", obj.Alias);
                writer.WriteLine("\t{");
                foreach (ObjectSchema mapSchema in list)
                {
                    ObjectSchema anotherParent = GetAnotherParent(obj, mapSchema);
                    if(anotherParent == null)
                        continue;

                    pkList = anotherParent.GetPKList();
                    if (pkList.Count != 1)
                        continue;

                    FieldSchema anotherParentPKField = pkList[0];

                    writer.WriteLine("\t\t{0}DataCollection GetByMapped{1}(Guid {2});", obj.Alias, anotherParent.Alias, anotherParentPKField.Alias);
                    writer.WriteLine("\t\t{0}ViewCollection GetViewByMapped{1}(Guid {2});", obj.Alias, anotherParent.Alias, anotherParentPKField.Alias);
               }
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private List<ObjectSchema> GetMapParents(ObjectSchema obj)
        {
            List<ObjectSchema> retList = new List<ObjectSchema>();
            foreach (ChildSchema item in obj.Children)
            {
                ObjectSchema childSchema = base.GetObjectByName(item.Name);
                List<FieldSchema> pkList = childSchema.GetPKList();
                if (pkList.Count != 2)
                    continue;

                retList.Add(childSchema);
            }

            return retList;
        }

        private ObjectSchema GetAnotherParent(ObjectSchema obj, ObjectSchema childSchema)
        {
            foreach (ParentSchema item in childSchema.Parents)
            {
                if(item.Alias == obj.Alias)
                    continue;

                return base.GetObjectByName(item.Name);
            }

            return null;
        }
    }
}
