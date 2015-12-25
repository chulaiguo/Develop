using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System.Collections;
using System.Text;

namespace JetCode.FactoryWinUI
{
    public class FactoryGetByUKInterface : FactoryBase
    {
        public FactoryGetByUKInterface(MappingSchema mappingSchema)
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
                List<Stack> list = new List<Stack>();
                Stack currentStack = new Stack();
                list.Add(currentStack);
                this.GetIndex(obj, currentStack, list);

                if (!this.IsWriteClass(list))
                    continue;

                writer.WriteLine("\tpublic partial interface I{0}DataService", obj.Alias);
                writer.WriteLine("\t{");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Count <= 1)
                        continue;

                    this.PrintMethod(i + 1, writer, obj, list[i]);
                    writer.WriteLine();
                }
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private bool IsWriteClass(List<Stack> list)
        {
            foreach (Stack item in list)
            {
                if (item.Count > 1)
                    return true;
            }

            return false;
        }

        private void PrintMethod(int index, StringWriter writer, ObjectSchema objSchema, Stack item)
        {
            object[] list = item.ToArray();
            if (list.Length == 0)
                return;

            writer.WriteLine("\t\t{0}Data GetByUK{1}({2});", objSchema.Alias, index, this.GetParas(list));
        }


        private void GetIndex(ObjectSchema objSchema, Stack currentStack, List<Stack> list)
        {
            int index = 0;
            Stack originalStack = currentStack.Clone() as Stack;
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                if (index >= 1)
                {
                    currentStack = originalStack.Clone() as Stack;
                    list.Add(currentStack);
                }

                FieldSchemaCollection pkFileds = this.AddReadableIndex(objSchema, item, currentStack);
                if (pkFileds.Count == 1)
                {
                    ObjectSchema parentSchema = this.GetParentSchema(objSchema, pkFileds[0]);
                    if (parentSchema == null)
                        continue;

                    this.GetIndex(parentSchema, currentStack, list);
                }

                index++;
            }
        }

        private ObjectSchema GetParentSchema(ObjectSchema objSchema, FieldSchema field)
        {
            foreach (ParentSchema parent in objSchema.Parents)
            {
                if (parent.LocalColumn != field.Name)
                    continue;

                ObjectSchema parentSchema = this.MappingSchema.Objects.Find(parent.Name);
                if (parentSchema == null)
                    continue;

                return parentSchema;
            }

            return null;
        }

        private FieldSchemaCollection AddReadableIndex(ObjectSchema objSchema, IndexSchema item, Stack currentStack)
        {
            FieldSchemaCollection retList = new FieldSchemaCollection();
            foreach (string key in item.Keys)
            {
                FieldSchema field = objSchema.Fields.Find(key);
                if (field == null)
                    break;

                if (field.DataType != "uniqueidentifier" || retList.Count > 0)
                {
                    currentStack.Push(field);
                }
                else
                {
                    retList.Add(field);
                }
            }

            return retList;
        }

        private string GetParas(object[] list)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < list.Length; i++)
            {
                FieldSchema field = list[i] as FieldSchema;
                if (field == null)
                    continue;

                Type fieldType = base.Utilities.ToDotNetType(field.DataType);
                builder.AppendFormat(" {0} {1},", fieldType.FullName, field.Alias);
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}
