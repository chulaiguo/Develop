using System;
using System.IO;
using JetCode.Factory;
using JetCode.BizSchema;
using System.Collections.Generic;

namespace JetCode.FactoryWinUI
{
    public class FactoryGroupDecorator : FactoryBase
    {
        private ObjectSchema _topParent = null;
        private ObjectSchema _leftParent = null;

        public FactoryGroupDecorator(MappingSchema mappingSchema, ObjectSchema objectSchema,
            ObjectSchema topParent, ObjectSchema leftParent)
            : base(mappingSchema, objectSchema)
        {
            this._topParent = topParent;
            this._leftParent = leftParent;
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.WinCtrl.Decoration;");
            writer.WriteLine("using Cheke.WinCtrl.GridGroupBuddy;");
            writer.WriteLine("using DevExpress.XtraGrid.Columns;");
            writer.WriteLine("using DevExpress.XtraGrid.Views.Grid;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.FormDetailEditor;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.GroupDecorator", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Group{0}{1}Decorator : GridGroupControlDecorator", this._topParent.Alias, this._leftParent.Alias);
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteConstruct(writer);

            List<FieldSchema> list = this.GetCommonFields();
            this.WriteLeftColumns(writer, list);
            this.WriteRightColumns(writer, list);

            this.WriteCompareEntity(writer);
            this.WriteCreateRightEntity(writer);
            this.WriteDetailFormType(writer);
            this.WriteReplaceEntity(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate {0} _parent = null;", this._topParent.Alias);
            writer.WriteLine();
            writer.WriteLine("\t\tpublic Group{0}{1}Decorator(string userId, GridGroupControl gridGroupControl)", this._topParent.Alias, this._leftParent.Alias);
            writer.WriteLine("\t\t\t: base(userId, gridGroupControl)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} Parent", this._topParent.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn this._parent;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis._parent = value;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteLeftColumns(StringWriter writer, List<FieldSchema> list)
        {
            writer.WriteLine("\t\tprotected override void SetLeftDisplayColumns(GridView view)");
            writer.WriteLine("\t\t{");

            foreach (FieldSchema item in list)
            {
                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", item.Alias, this._leftParent.Alias);
                if (item.Alias == "LastModifiedBy" || item.Alias == "LastModifiedAt"
                    || item.Alias == "CreatedOn" || item.Alias == "CreatedBy"
                    || item.Alias == "ModifiedOn" || item.Alias == "ModifiedBy")
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = -1;", item.Alias);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.ShowInCustomizationForm = false;", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", item.Alias);
                }
                writer.WriteLine("\t\t\tview.Columns.Add(col{0});", item.Alias);

                writer.WriteLine();
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteRightColumns(StringWriter writer, List<FieldSchema> list)
        {
            writer.WriteLine("\t\tprotected override void SetRightDisplayColumns(GridView view)");
            writer.WriteLine("\t\t{");

            foreach (FieldSchema item in list)
            {
                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", item.Alias, base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowEdit = false;", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowFocus = false;", item.Alias);

                if (item.Alias == "CreatedOn" || item.Alias == "CreatedBy"
                    || item.Alias == "ModifiedOn" || item.Alias == "ModifiedBy"
                    || item.Alias == "LastModifiedAt" || item.Alias == "LastModifiedBy")
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = -1;", item.Alias);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.ShowInCustomizationForm = false;", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", item.Alias);
                }
                writer.WriteLine("\t\t\tview.Columns.Add(col{0});", item.Alias);
                writer.WriteLine();
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCompareEntity(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override bool CompareEntity(BusinessBase left, BusinessBase right)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} {1} = left as {0};", this._leftParent.Alias, base.LowerFirstLetter(this._leftParent.Alias));
            writer.WriteLine("\t\t\t{0} {1} = right as {0};", base.ObjectSchema.Alias, base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\tif({0} == null || {1} == null)", base.LowerFirstLetter(this._leftParent.Alias), base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\treturn false;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn {0}.{2} == {1}.{2};", base.LowerFirstLetter(this._leftParent.Alias), 
                base.LowerFirstLetter(base.ObjectSchema.Alias), this.GetParentPK(this._leftParent));
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCreateRightEntity(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override BusinessBase CreateRightEntity(BusinessBase leftEntity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} {1} = leftEntity as {0};", this._leftParent.Alias, base.LowerFirstLetter(this._leftParent.Alias));
            writer.WriteLine("\t\t\tif({0} == null)", base.LowerFirstLetter(this._leftParent.Alias));
            writer.WriteLine("\t\t\t\treturn null;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0} right = new {0}();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tright.CopyParent({0});", base.LowerFirstLetter(this._leftParent.Alias));
            writer.WriteLine("\t\t\tright.CopyParent(this._parent);");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn right;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDetailFormType(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override Type DetailFormType");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn typeof (FormDetail{0});", this._leftParent.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteReplaceEntity(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic override bool ReplaceData(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (entity.GetType() == typeof({0}))", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} right = entity as {0};", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\tif (right == null || right.{0} != this._parent.{0})", this.GetParentPK(this._topParent));
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn base.ReplaceData(entity);");
            writer.WriteLine("\t\t}");
        }

        #region Helper functions
        private string GetParentPK(ObjectSchema schema)
        {
            foreach (FieldSchema item in schema.Fields)
            {
                if(item.IsPK)
                    return item.Alias;
            }

            return string.Empty;
        }

        private List<FieldSchema> GetCommonFields()
        {
            List<FieldSchema> list = new List<FieldSchema>();
            foreach (FieldSchema item in base.ObjectSchema.Fields)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(Guid))
                    continue;

                if (!dotnetType.IsValueType && dotnetType != typeof(string))
                    continue;

                if (this._leftParent.Fields.Find(item.Name) == null)
                    continue;

                list.Add(item);
            }

            return list;
        }
        #endregion
    }
}
