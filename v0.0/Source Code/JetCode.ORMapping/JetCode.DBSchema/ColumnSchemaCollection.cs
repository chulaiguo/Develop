using System.Collections;

namespace JetCode.DBSchema
{
    public class ColumnSchemaCollection : CollectionBase
    {
        public int Add(ColumnSchema item)
        {
            return base.List.Add(item);
        }

        public void Remove(ColumnSchema item)
        {
            base.List.Remove(item);
        }

        public ColumnSchema Find(string name)
        {
            foreach (ColumnSchema schema in base.List)
            {
                if (string.Compare(schema.Name, name, true) == 0)
                {
                    return schema;
                }
            }
            return null;
        }

        public ColumnSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public ColumnSchema this[int index]
        {
            get { return (ColumnSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}