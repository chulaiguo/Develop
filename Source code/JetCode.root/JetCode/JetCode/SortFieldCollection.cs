using System;
using System.Collections;

namespace JetCode
{
    [Serializable]
    public class SortFieldCollection : CollectionBase
    {
        public void Add(SortField entity)
        {
            base.List.Add(entity);
        }

        public void Add(string fieldAlias, bool asc)
        {
            this.Add(new SortField(fieldAlias, asc));
        }

        public void AddRange(SortFieldCollection list)
        {
            foreach (SortField field in list)
            {
                base.List.Add(field);
            }
        }

        public void Remove(SortField entity)
        {
            base.List.Remove(entity);
        }

        public SortField this[int index]
        {
            get { return (SortField) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}