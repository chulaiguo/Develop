using System.Collections;

namespace JetCode.BizSchema
{
    public class IndexSchemaCollection : CollectionBase
    {
        public int Add(IndexSchema item)
        {
            return base.List.Add(item);
        }

        public void Remove(IndexSchema item)
        {
            base.List.Remove(item);
        }

        public void AddRange(IndexSchemaCollection list)
        {
            foreach (IndexSchema item in list)
            {
                this.Add(item);
            }
        }

        public IndexSchema Find(string name)
        {
            foreach (IndexSchema schema in base.List)
            {
                if (string.Compare(schema.Name, name, true) == 0)
                {
                    return schema;
                }
            }
            return null;
        }

        public IndexSchemaCollection FindByKeysString(string keys)
        {
            IndexSchemaCollection list = new IndexSchemaCollection();
            foreach (IndexSchema schema in base.List)
            {
                if (string.Compare(schema.KeysString, keys, true) == 0)
                {
                    list.Add(schema);
                }
            }

            return list;
        }

        public IndexSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public IndexSchema this[int index]
        {
            get { return (IndexSchema)base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}