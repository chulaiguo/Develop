using System.Collections;

namespace JetCode.DBSchema
{
    public class DBIndexSchemaCollection : CollectionBase
    {
        public int Add(DBIndexSchema item)
        {
            return base.List.Add(item);
        }

        public void Remove(DBIndexSchema item)
        {
            base.List.Remove(item);
        }

        public DBIndexSchema Find(string name)
        {
            foreach (DBIndexSchema schema in base.List)
            {
                if (string.Compare(schema.Name, name, true) == 0)
                {
                    return schema;
                }
            }
            return null;
        }

        public DBIndexSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public DBIndexSchema this[int index]
        {
            get { return (DBIndexSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}