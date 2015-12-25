using System.Collections;

namespace JetCode.BizSchema
{
    public class ChildSchemaCollection : CollectionBase
    {
        public int Add(ChildSchema item)
        {
            return base.List.Add(item);
        }

        public void AddRange(ChildSchemaCollection list)
        {
            foreach (ChildSchema item in list)
            {
                this.Add(item);
            }
        }

        public ChildSchema Find(string name)
        {
            foreach (ChildSchema item in base.List)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public void Remove(ChildSchema item)
        {
            base.List.Remove(item);
        }

        public ChildSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public ChildSchema this[int index]
        {
            get { return (ChildSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}