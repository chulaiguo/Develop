using System.Collections;

namespace JetCode.BizSchema
{
    public class ObjectSchemaCollection : CollectionBase
    {
        public int Add(ObjectSchema item)
        {
            return base.List.Add(item);
        }

        public void Remove(ObjectSchema item)
        {
            base.List.Remove(item);
        }

        public void AddRange(ObjectSchemaCollection list)
        {
            foreach (ObjectSchema item in list)
            {
                this.Add(item);
            }
        }

        public void RemoveRange(ObjectSchemaCollection list)
        {
            foreach (ObjectSchema item in list)
            {
                this.Remove(item);
            }
        }

        public ObjectSchema Find(string name)
        {
            foreach (ObjectSchema item in base.List)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public ObjectSchemaCollection FindByDBName(string dbName)
        {
            ObjectSchemaCollection list = new ObjectSchemaCollection();
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (string.Compare(this[i].DBName, dbName, true) == 0)
                    list.Add(this[i]);
            }

            return list;
        }

        public ObjectSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public ObjectSchema this[int index]
        {
            get { return (ObjectSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}