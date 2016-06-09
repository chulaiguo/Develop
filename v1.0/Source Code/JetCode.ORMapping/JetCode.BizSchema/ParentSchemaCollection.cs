using System.Collections;

namespace JetCode.BizSchema
{
    public class ParentSchemaCollection : CollectionBase
    {
        public int Add(ParentSchema item)
        {
            return base.List.Add(item);
        }

        public void AddRange(ParentSchemaCollection list)
        {
            foreach (ParentSchema item in list)
            {
                this.Add(item);
            }
        }

        public ParentSchema Find(string name)
        {
            foreach (ParentSchema item in base.List)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public void Remove(ParentSchema item)
        {
            base.List.Remove(item);
        }

        public ParentSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public ParentSchema this[int index]
        {
            get { return (ParentSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}