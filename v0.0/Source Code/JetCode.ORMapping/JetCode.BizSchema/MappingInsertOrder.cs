using System.Collections.Specialized;

namespace JetCode.BizSchema
{
    public class MappingInsertOrder
    {
        private MappingSchema _mapingSchema = null;

        public MappingInsertOrder(MappingSchema mappingSchema)
        {
            this._mapingSchema = mappingSchema;
        }

        private ObjectSchemaCollection Objects
        {
            get { return this._mapingSchema.Objects; }
        }

        public StringCollection GetInsertOrder()
        {
            StringCollection list = new StringCollection();
            this.FindTopParent(list);

            while (true)
            {
                foreach (ObjectSchema item in this.Objects)
                {
                    if(list.Contains(item.Name))
                        continue;

                    if(this.IsParentInList(item, list))
                    {
                        list.Add(item.Name);
                    }
                }

                if(list.Count == this.Objects.Count)
                    break;
            }

            return list;
        }

        private void FindTopParent(StringCollection list)
        {
            foreach (ObjectSchema item in this.Objects)
            {
                if(item.Parents.Count == 0)
                {
                    list.Add(item.Name);
                }
            }
        }

        private bool IsParentInList(ObjectSchema child, StringCollection list)
        {
            foreach (ParentSchema item in child.Parents)
            {
                if(!list.Contains(item.Name))
                    return false;
            }

            return true;
        }
    }
}