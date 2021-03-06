using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryViewCollection : FactoryBase
    {
        public FactoryViewCollection(MappingSchema mappingSchema, ObjectSchema objectSchema) 
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using Cheke.BusinessEntity;"); 

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", this.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic class {0}ViewCollection : CollectionBase", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate CollectionBlock _block = new CollectionBlock();");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic {0}ViewCollection()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}ViewCollection({0}DataCollection list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0}Data item in list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.List.Add(new {0}View(item));", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t\tthis._block.Size = list.Block.Size;");
            writer.WriteLine("\t\t\tthis._block.Index = list.Block.Index;");
            writer.WriteLine("\t\t\tthis._block.Count = list.Block.Count;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic CollectionBlock Block");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{ return this._block; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}DataCollection To{0}DataCollection()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DataCollection retList = new {0}DataCollection();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tforeach ({0}View item in base.List)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList.Add(item.To{0}Data());", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tretList.AcceptChanges();");
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Add({0}View entity)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.List.Add(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Insert(int index, {0}View entity)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.List.Insert(index, entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Remove({0}View entity)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.List.Remove(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void AddRange({0}ViewCollection list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0}View item in list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.List.Add(item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}View this[int index]", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{{ return ({0}View)base.List[index]; }}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tset{ base.List[index] = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}ViewCollection SubCollection()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}ViewCollection retList = new {0}ViewCollection();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (this.Count <= this.Block.Size)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList.AddRange(this);");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tretList.Block.Size = this.Block.Size;");
            writer.WriteLine("\t\t\t\tretList.Block.Count = 1;");
            writer.WriteLine("\t\t\t\tretList.Block.Index = 0;");
            writer.WriteLine("\t\t\t\treturn retList;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tint start = this.Block.Index * this.Block.Size;");
            writer.WriteLine("\t\t\tint count = this.Block.Size;");
            writer.WriteLine("\t\t\tfor (int i = start; i < (start + count); i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (i >= this.Count)");
            writer.WriteLine("\t\t\t\t\tbreak;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tretList.Add(this[i]);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tretList.Block.Size = this.Block.Size;");
            writer.WriteLine("\t\t\tretList.Block.Index = this.Block.Index;");
            writer.WriteLine("\t\t\tretList.Block.Count = (this.Count / this.Block.Size) + (this.Count % this.Block.Size == 0 ? 0 : 1);");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
        }
    }
}
