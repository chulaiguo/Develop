using System.Collections;

namespace JetCode.BizSchema
{
    public class FieldSchemaCollection : CollectionBase
    {
        public int Add(FieldSchema item)
        {
            return base.List.Add(item);
        }

        public void AddRange(FieldSchemaCollection list)
        {
            foreach (FieldSchema item in list)
            {
                this.Add(item);
            }
        }

        public FieldSchema Find(string columnName)
        {
            foreach (FieldSchema item in base.List)
            {
                if (string.Compare(item.Name, columnName, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public void Remove(FieldSchema item)
        {
            base.List.Remove(item);
        }

        public FieldSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public FieldSchema this[int index]
        {
            get { return (FieldSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}