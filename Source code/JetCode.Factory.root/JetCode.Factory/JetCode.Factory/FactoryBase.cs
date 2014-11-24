using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetCode.BizSchema;

namespace JetCode.Factory
{
    public abstract class FactoryBase
    {
        private static DatabaseTypeCode _databaseType = DatabaseTypeCode.Default;

        private MappingSchema _mappingSchema = null;
        private ObjectSchema _objectSchema = null;

        public FactoryBase(MappingSchema mappingSchema)
        {
            this._mappingSchema = mappingSchema;
        }

        public FactoryBase(MappingSchema mappingSchema, ObjectSchema objectSchema)
        {
            this._mappingSchema = mappingSchema;
            this._objectSchema = objectSchema;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        protected abstract void BeginWrite(StringWriter writer);
        protected abstract void WriteUsing(StringWriter writer);
        protected abstract void WriteContent(StringWriter writer);
        protected abstract void EndWrite(StringWriter writer);

        protected ObjectSchema ObjectSchema
        {
            get { return _objectSchema; }
        }

        protected MappingSchema MappingSchema
        {
            get { return _mappingSchema; }
        }

        protected string ProjectName
        {
            get { return this._mappingSchema.Name; }
        }

        protected string LowerFirstLetter(string str)
        {
            return string.Format("{0}{1}", str.Substring(0, 1).ToLower(), str.Substring(1));
        }

        protected UtilityBase Utilities
        {
            get
            {
                if (DatabaseType == DatabaseTypeCode.Oracle)
                {
                    return new OracleUtility();
                }
                 
                return new MSSQLUtility();
            }
        }

        public static DatabaseTypeCode DatabaseType
        {
            get { return _databaseType; }
            set { _databaseType = value; }
        }

        protected FieldSchema GetLogicDeleteField(ObjectSchema entity)
        {
            foreach (FieldSchema field in entity.Fields)
            {
                if (string.Compare(field.Alias, "Deleted", true) == 0)
                    return field;

                if (string.Compare(field.Alias, "Deactive", true) == 0)
                    return field;

                if (string.Compare(field.Alias, "Inactive", true) == 0)
                    return field;
            }

            return null;
        }

        #region Schema Helper Functions

        protected bool IsFKField(FieldSchema field)
        {
            foreach (ParentSchema item in this.ObjectSchema.Parents)
            {
                if (item.LocalColumn == field.Name)
                    return true;
            }

            return false;
        }

        protected bool IsMapTable()
        {
            return IsMapTable(this.ObjectSchema);
        }

        public static bool IsMapTable(ObjectSchema objSchema)
        {
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                bool isFK = true;
                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field == null)
                        break;

                    isFK = IsParentField(field.Name, objSchema.Parents);
                    if(!isFK)
                    {
                        break;
                    }
                }

                if(isFK)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsParentField(string fieldName, ParentSchemaCollection list)
        {
            foreach (ParentSchema parent in list)
            {
                if (parent.LocalColumn == fieldName)
                {
                    return true;
                }
            }

            return false;
        }

        protected FieldSchemaCollection GetUKFields()
        {
            return this.GetUKFields(this.ObjectSchema);
        }

        protected FieldSchemaCollection GetReadableUKFields()
        {
            return this.GetReadableUKFields(this.ObjectSchema);
        }

        protected FieldSchemaCollection GetUKFields(string name)
        {
            ObjectSchema objSchema = this.MappingSchema.Objects.Find(name);
            if(objSchema == null)
                return new FieldSchemaCollection();

            return this.GetUKFields(objSchema);
        }

        protected FieldSchemaCollection GetReadableUKFields(string name)
        {
            ObjectSchema objSchema = this.MappingSchema.Objects.Find(name);
            if (objSchema == null)
                return new FieldSchemaCollection();

            return this.GetReadableUKFields(objSchema);
        }

        private FieldSchemaCollection GetUKFields(ObjectSchema objSchema)
        {
            FieldSchemaCollection list = new FieldSchemaCollection();
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field != null)
                    {
                        list.Add(field);
                    }
                }
            }

            return list;
        }

        private FieldSchemaCollection GetReadableUKFields(ObjectSchema objSchema)
        {
            FieldSchemaCollection list = new FieldSchemaCollection();
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field == null)
                        break;

                    bool isFK = false;
                    foreach (ParentSchema parent in objSchema.Parents)
                    {
                        if (parent.LocalColumn != field.Name)
                            continue;

                        isFK = true;
                        ObjectSchema parentSchema = this.MappingSchema.Objects.Find(parent.Name);
                        if(parentSchema != null)
                        {
                            FieldSchemaCollection children = this.GetReadableUKFields(parentSchema);
                            list.AddRange(children);
                            break;
                        }
                    }

                    if (!isFK)
                    {
                        list.Add(field);
                    }
                }
            }

            return list;
        }

        protected FieldSchemaCollection GetRequiredFields()
        {
            FieldSchemaCollection list = new FieldSchemaCollection();
            foreach (FieldSchema item in this.ObjectSchema.Fields)
            {
                if (item.IsPK || item.IsJoined)
                    continue;

                if (!item.IsNullable)
                    continue;

                list.Add(item);
            }

            return list;
        }

        protected FieldSchemaCollection GetRequiredFields(string name)
        {
            FieldSchemaCollection list = new FieldSchemaCollection();
            ObjectSchema objSchema = this.MappingSchema.GetObjectByName(name);
            if (objSchema == null)
                return list;

            foreach (FieldSchema item in objSchema.Fields)
            {
                if (item.IsPK || item.IsJoined)
                    continue;

                if (!item.IsNullable)
                    continue;

                list.Add(item);
            }

            return list;
        }

        protected FieldSchemaCollection GetSearchableFields()
        {
            FieldSchemaCollection list = new FieldSchemaCollection();
            foreach (FieldSchema item in this.ObjectSchema.Fields)
            {
                //if(item.Name.Contains("Password"))
                //    continue;

                //if (string.Compare(item.Name, "CreatedOn", true) == 0 ||
                //    string.Compare(item.Name, "CreatedBy", true) == 0 ||
                //    string.Compare(item.Name, "ModifiedOn", true) == 0 ||
                //    string.Compare(item.Name, "ModifiedBy", true) == 0 )
                //    continue;

                //if (item.IsPK || this.IsFKField(item) || this.IsUKField(item))
                //    continue;

                //Type fieldType = this.Utilities.ToDotNetType(item.DataType);
                //if (fieldType == typeof(string))
                //{
                //    int size;
                //    if(int.TryParse(item.Size, out size) && size <= 256)
                //    {
                //        list.Add(item);
                //    }

                //    continue;  
                //}

                //if (fieldType == typeof(Guid) || fieldType == typeof(DateTime) ||
                //    fieldType == typeof(byte) || fieldType == typeof(short) ||
                //    fieldType == typeof(int) || fieldType == typeof(long) ||
                //    fieldType == typeof(float) || fieldType == typeof(double) ||
                //    fieldType == typeof(decimal) || fieldType == typeof(bool))
                //{
                //    list.Add(item);
                //}

                if (!item.IsJoined)
                    continue;

                Type fieldType = this.Utilities.ToDotNetType(item.DataType);
                if (fieldType == typeof(string))
                {
                    int size;
                    if (!int.TryParse(item.Size, out size) || size > 128)
                        continue;
                }

                list.Add(item);
            }

            return list;
        }

        protected FieldSchemaCollection GetBindableFields()
        {
            FieldSchemaCollection list = new FieldSchemaCollection();
            foreach (FieldSchema item in this.ObjectSchema.Fields)
            {
                if (item.IsJoined)
                    continue;

                if (item.Name.Contains("Password") || item.Name.Contains("Passwd"))
                    continue;

                if (string.Compare(item.Name, "CreatedOn", true) == 0 ||
                    string.Compare(item.Name, "CreatedBy", true) == 0 ||
                    string.Compare(item.Name, "ModifiedOn", true) == 0 ||
                    string.Compare(item.Name, "ModifiedBy", true) == 0 ||
                    string.Compare(item.Name, "LastModifyBy", true) == 0 ||
                    string.Compare(item.Name, "LastModifyAt", true) == 0 ||
                    string.Compare(item.Name, "LastModifiedBy", true) == 0 ||
                    string.Compare(item.Name, "LastModifiedAt", true) == 0)
                    continue;

                Type fieldType = this.Utilities.ToDotNetType(item.DataType);
                if (item.IsPK && fieldType == typeof(Guid))
                    continue;

                if (fieldType == typeof(string) || fieldType == typeof(DateTime) ||
                    fieldType == typeof(byte) || fieldType == typeof(short) ||
                    fieldType == typeof(int) || fieldType == typeof(long) ||
                    fieldType == typeof(float) || fieldType == typeof(double) ||
                    fieldType == typeof(decimal) || fieldType == typeof(bool))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        protected string GetPKDeclareParameters()
        {
            StringBuilder builder = new StringBuilder();
            foreach (FieldSchema item in this.ObjectSchema.Fields)
            {
                if (!item.IsPK)
                    continue;

                builder.AppendFormat(" {0} {1},", this.Utilities.ToDotNetType(item.DataType), item.Name);
            }

            return builder.ToString().TrimEnd(',');
        }

        protected string GetPKInputParameters()
        {
            return this.GetPKInputParameters(string.Empty);
        }

        protected string GetPKInputParameters(string preName)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FieldSchema item in this.ObjectSchema.Fields)
            {
                if (!item.IsPK)
                    continue;

                if (preName.Length > 0)
                {
                    builder.AppendFormat(" {0}.{1},", preName, item.Name);
                }
                else
                {
                    builder.AppendFormat(" {0},", item.Name);
                }
            }

            return builder.ToString().TrimEnd(',');
        }

        protected ParentSchemaCollection GetParentList()
        {
            ParentSchemaCollection list = new ParentSchemaCollection();
            this._mappingSchema.GetParentList(this.ObjectSchema, list);
            return list;
        }

        protected ObjectSchema GetObjectByName(string name)
        {
            return this._mappingSchema.GetObjectByName(name);
        }

        #endregion
    }
}