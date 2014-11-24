using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryData : FactoryBase
    {
        public FactoryData(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using Cheke.BusinessEntity;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {   
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t[Serializable]");
                writer.WriteLine("\tpublic partial class {0}Data : BusinessBase", item.Alias);
                writer.WriteLine("\t{");

                this.WriteFields(writer, item);
                this.WriteConstructor(writer, item);
                this.WriteProperties(writer, item);

                this.WriteCopyFromMethod(writer, item);
                this.WriteCloneChildrenMethod(writer, item);
                this.WriteAcceptChangesMethod(writer, item);
                this.WriteEqualMethod(writer, item);
                this.WriteGetHashCodeMethod(writer, item);
                this.WriteToStringMethod(writer, item);
                this.WriteIsDirty(writer, item);
                this.WriteIsValid(writer, item);
                this.WriteBindingEvent(writer, item);
                
                writer.WriteLine("\t}");
            }
        }

        private void WriteFields(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Fields");
            foreach(FieldSchema item in obj.Fields)
            {
                writer.WriteLine("\t\tprivate {0} _{1};", Utilities.ToDotNetType(item.DataType).Name, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine();
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tprivate {0}DataCollection _{1}List;", item.Alias, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Constructors");

            writer.WriteLine("\t\tpublic {0}Data()", obj.Alias);
            writer.WriteLine("\t\t{");
            foreach (FieldSchema item in obj.Fields)
            {
                Type dotNetType = Utilities.ToDotNetType(item.DataType);
                if (dotNetType == typeof(Guid))
                {
                    if (item.IsPK)
                    {
                        writer.WriteLine("\t\t\tthis._{0} = Guid.NewGuid();", base.LowerFirstLetter(item.Alias));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tthis._{0} = Guid.Empty;", base.LowerFirstLetter(item.Alias));
                    }
                    continue;
                }

                if (dotNetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis._{0} = DateTime.Now;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(string))
                {
                    writer.WriteLine("\t\t\tthis._{0} = string.Empty;", base.LowerFirstLetter(item.Alias));
                    continue;
                }
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Data({0}Data data)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.CopyFrom(data, true);");
            writer.WriteLine("\t\t\tthis.CloneChildren(data);");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Properties");
            //Field
            foreach (FieldSchema item in obj.Fields)
            {
                writer.WriteLine("\t\tpublic virtual {0} {1}", Utilities.ToDotNetType(item.DataType).Name, item.Alias);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this._{0};", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                if (item.IsJoined)
                {
                    writer.WriteLine("\t\t\t\tthis._{0} = value;", base.LowerFirstLetter(item.Alias));
                }
                else
                {
                    writer.WriteLine("\t\t\t\tEntityEventArgs e = new EntityEventArgs(\"{0}\", value);", item.Alias);
                    writer.WriteLine("\t\t\t\tif (this._{0} != value)", base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tthis.OnBeforeChanged(e);");
                    writer.WriteLine("\t\t\t\t\tif (!e.Cancel)");
                    writer.WriteLine("\t\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t\tthis._{0} = value;", base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\t\t\tthis.CheckRules(\"{0}\");", item.Alias);
                    writer.WriteLine("\t\t\t\t\t\tbase.MarkDirty();");
                    writer.WriteLine("\t\t\t\t\t\tthis.OnAfterChanged(e);");
                    writer.WriteLine("\t\t\t\t\t\tthis.On{0}Changed(EventArgs.Empty);", item.Alias);
                    writer.WriteLine("\t\t\t\t\t}");
                    writer.WriteLine("\t\t\t\t}");
                }
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            //Children
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tpublic virtual {0}DataCollection {0}List", item.Alias);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this._{0}List;", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._{0}List = value;", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            //Table Name
            writer.WriteLine("\t\tpublic override string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"{0}\";", obj.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteCopyFromMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override void CopyFrom(BusinessBase entity, bool all)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = entity as {0}Data;", obj.Alias);
            writer.WriteLine("\t\t\tif (data == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsPK)
                    continue;

                writer.WriteLine("\t\t\tthis.{0} = data.{0};", item.Alias);
            }

            writer.WriteLine("\t\t\tif (all)");
            writer.WriteLine("\t\t\t{");
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsPK)//set PK
                {
                    writer.WriteLine("\t\t\t\tthis.{0} = data.{0};", item.Alias);
                }
            }
            writer.WriteLine("\t\t\t\tbase._objectID = data.ObjectID;");
            writer.WriteLine("\t\t\t\tbase.MarkOld();");
            writer.WriteLine("\t\t\t\tif (data.IsNew)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tif (data.IsDeleted)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkDeleted();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tif (data.IsSelfDirty)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkDirty();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCloneChildrenMethod(StringWriter writer, ObjectSchema obj)
        {
            if (obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tprotected override void CloneChildren(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = entity as {0}Data;", obj.Alias);
            writer.WriteLine("\t\t\tif (data == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\tif (data.{0}List != null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List = new {0}DataCollection(data.{0}List);", item.Alias);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteAcceptChangesMethod(StringWriter writer, ObjectSchema obj)
        {
            if (obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tpublic override void AcceptChanges()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.AcceptChanges();");
            writer.WriteLine();

            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\tif (this.{0}List != null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List.AcceptChanges();", item.Alias);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEqualMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override bool Equals(object obj)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn ((obj.GetType() == this.GetType()) && this.InternalEquals(obj as {0}Data));", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate bool InternalEquals({0}Data obj)", obj.Alias);
            writer.WriteLine("\t\t{");

            foreach (FieldSchema item in obj.Fields)
            {
                if(!item.IsPK)
                    continue;

                writer.WriteLine("\t\t\tif (this.{0}.CompareTo(obj.{0}) != 0)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t\treturn true;");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetHashCodeMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override int GetHashCode()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.ToString().GetHashCode();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteBindingEvent(StringWriter writer, ObjectSchema obj)
        {
           writer.WriteLine("\t\t#region Binding Events");
            //Field
            foreach (FieldSchema item in obj.Fields)
            {
                if(item.IsJoined)
                    continue;

                writer.WriteLine("\t\t[field: NonSerialized, NotUndoable]");
                writer.WriteLine("\t\tpublic event EventHandler {0}Changed;", item.Alias);
               
                writer.WriteLine("\t\tprivate void On{0}Changed(EventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif ((this.{0}Changed != null))", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}Changed(this, e);", item.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteToStringMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override string PKString");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tStringBuilder builder = new StringBuilder();");
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsPK)
                    continue;

                writer.WriteLine("\t\t\t\tbuilder.AppendFormat(\"{{0}}|\", this.{0});", item.Alias);
            }
            writer.WriteLine("\t\t\t\treturn builder.ToString().TrimEnd('|');");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override string ToString()");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            writer.WriteLine("\t\t\tbuilder.Append(\"{0}:\");", obj.Alias);
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsPK)
                    continue;

                writer.WriteLine("\t\t\tbuilder.AppendFormat(\"{{0}}|\", this.{0});", item.Alias);
            }
            writer.WriteLine("\t\t\treturn builder.ToString().TrimEnd('|');");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteIsDirty(StringWriter writer, ObjectSchema obj)
        {
            if(obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tpublic override bool IsDirty");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif(base.IsDirty)");
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine();
            
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\t\tif (this.{0}List != null && this.{0}List.IsDirty)", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn true;");
                writer.WriteLine("\t\t\t\t}");
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\t\treturn false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteIsValid(StringWriter writer, ObjectSchema obj)
        {
            if (obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tpublic override bool IsValid");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif(!base.IsValid)");
            writer.WriteLine("\t\t\t\t\treturn false;");
            writer.WriteLine();

            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\t\tif (this.{0}List != null && !this.{0}List.IsValid)", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn false;");
                writer.WriteLine("\t\t\t\t}");
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}