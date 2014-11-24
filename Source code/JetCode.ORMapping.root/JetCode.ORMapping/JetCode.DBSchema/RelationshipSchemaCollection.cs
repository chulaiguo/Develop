using System.Collections;

namespace JetCode.DBSchema
{
    public class RelationshipSchemaCollection : CollectionBase
    {
        public int Add(RelationshipSchema item)
        {
            return base.List.Add(item);
        }

        public void Remove(RelationshipSchema item)
        {
            base.List.Remove(item);
        }

        public RelationshipSchema Find(string name)
        {
            foreach (RelationshipSchema schema in base.List)
            {
                if (string.Compare(schema.Name, name, true) == 0)
                {
                    return schema;
                }
            }
            return null;
        }

        public RelationshipSchema this[string name]
        {
            get { return this.Find(name); }
        }

        public RelationshipSchema this[int index]
        {
            get { return (RelationshipSchema) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}