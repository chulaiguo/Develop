using System.Collections;

namespace JetCode.DBSchema
{
    public class TableSchemaCollection : CollectionBase
    {
        public int Add(TableSchema item)
        {
            return base.List.Add(item);
        }

        public TableSchema Find(string name)
        {
            foreach (TableSchema schema in base.List)
            {
                if (string.Compare(schema.Name, name, true) == 0)
                {
                    return schema;
                }
            }
            return null;
        }

        public void Remove(TableSchema item)
        {
            base.List.Remove(item);
        }

        public TableSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public TableSchema this[int index]
        {
            get { return (TableSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}