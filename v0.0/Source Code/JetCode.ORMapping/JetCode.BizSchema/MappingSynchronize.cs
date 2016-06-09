using JetCode.BizSchema.Factory;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class MappingSynchronize
    {
        private MappingSchema _mapingSchema = null;

        public MappingSynchronize(MappingSchema mappingSchema)
        {
            this._mapingSchema = mappingSchema;
        }

        private ObjectSchemaCollection Objects
        {
            get { return this._mapingSchema.Objects; }
        }

        private DatabaseSchema GetDatabaseSchema(string connString, string database)
        {
            return SchemaFactory.GetDatabaseSchema(connString, database);
        }

        public void Synchronize(string connString, string database)
        {
            DatabaseSchema dbSchema = this.GetDatabaseSchema(connString, database);

            ObjectSchemaCollection oldList = this.Objects.FindByDBName(dbSchema.Name);
            this.Objects.RemoveRange(oldList);

            foreach (TableSchema item in dbSchema.Tables)
            {
                ObjectSchema newSchema = ObjectSchema.LoadFromSchema(dbSchema.Name, item);
                ObjectSchema oldSchema = oldList.Find(item.Name);
                if (oldSchema != null)
                {
                    this.Synchronize(oldSchema, newSchema);
                }

                this.Objects.Add(newSchema);
            }
        }

        public void AddNewSchema(string connString, string database)
        {
            DatabaseSchema dbSchema = SchemaFactory.GetDatabaseSchema(connString, database);
            foreach (TableSchema item in dbSchema.Tables)
            {
                ObjectSchema newSchema = ObjectSchema.LoadFromSchema(dbSchema.Name, item);
                this.Objects.Add(newSchema);
            }
        }

        private void Synchronize(ObjectSchema oldSchema, ObjectSchema newSchema)
        {
            this.SynchronizeFields(oldSchema, newSchema);
            this.SynchronizeJoins(oldSchema, newSchema);
            this.SynchronizeChildren(oldSchema, newSchema);
            this.SynchronizeParents(oldSchema, newSchema);
            this.SynchronizeIndexs(oldSchema, newSchema);
        }

        private void SynchronizeFields(ObjectSchema oldSchema, ObjectSchema newSchema)
        {
            //Remain old alias setting
            foreach (FieldSchema field in newSchema.Fields)
            {
                FieldSchema oldField = oldSchema.Fields.Find(field.Name);
                if (oldField != null)
                {
                    field.Alias = oldField.Alias;
                    if(!field.IsPK && field.IsNullable)
                    {
                        field.IsNullable = oldField.IsNullable;
                    }
                }
            }

            //Remain old join fields setting
            foreach (FieldSchema field in oldSchema.Fields)
            {
                if (!field.IsJoined)
                    continue;

                newSchema.Fields.Add(field);
            }
        }

        private void SynchronizeJoins(ObjectSchema oldSchema, ObjectSchema newSchema)
        {
            newSchema.Joins.Clear();
            foreach (JoinSchema join in oldSchema.Joins)
            {
                newSchema.Joins.Add(join);
            }
        }

        private void SynchronizeChildren(ObjectSchema oldSchema, ObjectSchema newSchema)
        {
            foreach (ChildSchema child in newSchema.Children)
            {
                ChildSchema oldChild = oldSchema.Children.Find(child.Name);
                if (oldChild != null)
                {
                    child.CascadeDelete = oldChild.CascadeDelete;
                }
            }
        }

        private void SynchronizeParents(ObjectSchema oldSchema, ObjectSchema newSchema)
        {
        }

        private void SynchronizeIndexs(ObjectSchema oldSchema, ObjectSchema newSchema)
        {
        }
    }
}