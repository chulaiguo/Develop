using System.Collections;

namespace JetCode.BizSchema
{
    public class JoinSchemaCollection : CollectionBase
    {
        public int Add(JoinSchema item)
        {
            return base.List.Add(item);
        }

        public void AddRange(JoinSchemaCollection list)
        {
            foreach (JoinSchema item in list)
            {
                this.Add(item);
            }
        }

        public JoinSchema Find(string refAlias)
        {
            foreach (JoinSchema item in base.List)
            {
                if (string.Compare(item.RefAlias, refAlias, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public void Remove(JoinSchema item)
        {
            base.List.Remove(item);
        }

        public JoinSchema this[string refAlias]
        {
            get { return this.Find(refAlias); }
        }

        public JoinSchema this[int index]
        {
            get { return (JoinSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}