using System;
using System.Collections;
using System.Text;

namespace JetCode.BusinessEntity
{
    [Serializable]
    public abstract class BusinessCollectionBase : CollectionBase, ICloneable
    {
        public abstract Type ItemType { get; }

        private readonly CollectionBlock _blcok = new CollectionBlock();

        public CollectionBlock Block
        {
            get { return this._blcok; }
        }

        private readonly ArrayList _deletedList = new ArrayList();

        public ArrayList DeletedList
        {
            get { return this._deletedList; }
        }

        public int IndexOf(object item)
        {
            if (item == null || item.GetType() != this.ItemType)
                return -1;

            for (int i = 0; i < this.InnerList.Count; i++)
            {
                object entity = this.InnerList[i];
                if (entity.Equals(item))
                    return i;
            }

            return -1;
        }

        protected override void OnRemove(int index, object value)
        {
            BusinessBase entity = value as BusinessBase;
            if (entity != null && !entity.IsNew)
            {
                this.DeletedList.Add(entity);
            }

            base.OnRemove(index, value);
        }

        protected override void OnClear()
        {
            foreach (object item in base.InnerList)
            {
                BusinessBase entity = item as BusinessBase;
                if (entity == null || entity.IsNew)
                    continue;

                this.DeletedList.Add(entity);
            }

            base.OnClear();
        }

        public void AcceptChanges()
        {
            foreach (object item in base.InnerList)
            {
                BusinessBase entity = item as BusinessBase;
                if (entity == null)
                    continue;

                entity.AcceptChanges();
            }
            this._deletedList.Clear();
        }

        public void AcceptChanges(Guid objId)
        {
            for (int i = this._deletedList.Count - 1; i >= 0; i--)
            {
                BusinessBase entity = this._deletedList[i] as BusinessBase;
                if (entity == null)
                    continue;

                if (entity.ObjectID == objId)
                {
                    this._deletedList.RemoveAt(i);
                    return;
                }
            }

            foreach (object item in base.InnerList)
            {
                BusinessBase entity = item as BusinessBase;
                if (entity == null)
                    continue;

                if (entity.ObjectID == objId)
                {
                    entity.AcceptChanges();
                    break;
                }
            }
        }

        public void AcceptDeletes(Result r)
        {
            for (int i = this.DeletedList.Count - 1; i >= 0; i--)
            {
                BusinessBase entity = this.DeletedList[i] as BusinessBase;
                if (entity == null)
                    continue;

                byte[] rowVersion = r.RowVersions[entity.ObjectID] as byte[];
                if (rowVersion != null && rowVersion.Length == 0)
                {
                    this.DeletedList.RemoveAt(i);
                }
            }
        }

        public void GetChanges()
        {
            if (!this.IsDirty)
            {
                this.Clear();
                this.AcceptChanges();
            }
            else
            {
                foreach (object item in this._deletedList)
                {
                    BusinessBase entity = item as BusinessBase;
                    if (entity == null)
                        continue;

                    entity.Delete();
                    this.InnerList.Insert(0, entity);
                }

                for (int i = this.InnerList.Count - 1; i >= 0; i--)
                {
                    BusinessBase entity = this.InnerList[i] as BusinessBase;
                    if (entity == null)
                        continue;

                    if (!entity.IsDirty)
                    {
                        this.InnerList.RemoveAt(i);
                    }
                }

                this._deletedList.Clear();
                foreach (object item in base.InnerList)
                {
                    BusinessBase entity = item as BusinessBase;
                    if (entity == null)
                        continue;

                    entity.GetChanges();
                }
            }
        }

        public string GetBrokenRules()
        {
            StringBuilder builder = new StringBuilder();
            foreach (object item in base.InnerList)
            {
                BusinessBase entity = item as BusinessBase;
                if (entity == null)
                    continue;

                string error = entity.GetBrokenRules();
                if (error.Length == 0)
                    continue;

                builder.AppendLine(error);
            }

            return builder.ToString();
        }


        public object Clone()
        {
            BusinessCollectionBase list = (BusinessCollectionBase) Activator.CreateInstance(this.GetType());
            foreach (object item in this.DeletedList)
            {
                BusinessBase entity = item as BusinessBase;
                if (entity == null)
                    continue;

                list.DeletedList.Add(entity.Clone());
            }

            foreach (object item in base.InnerList)
            {
                BusinessBase entity = item as BusinessBase;
                if (entity == null)
                    continue;

                list.InnerList.Add(entity.Clone());
            }

            return list;
        }


        public bool IsDirty
        {
            get
            {
                if (this.DeletedList.Count > 0)
                {
                    return true;
                }

                foreach (object item in base.InnerList)
                {
                    BusinessBase entity = item as BusinessBase;
                    if (entity == null)
                        continue;

                    if (entity.IsDirty)
                        return true;
                }

                return false;
            }
        }

        public bool IsValid
        {
            get
            {
                foreach (object item in base.InnerList)
                {
                    BusinessBase entity = item as BusinessBase;
                    if (entity == null)
                        continue;

                    if (!entity.IsValid)
                        return false;
                }

                return true;
            }
        }

        public BusinessCollectionBase SubCollection()
        {
            int start = this._blcok.Index*this._blcok.Size;
            int size = this._blcok.Size;
            this._blcok.Count = (this.InnerList.Count/this._blcok.Size) +
                                (((this.InnerList.Count%this._blcok.Size) == 0) ? 0 : 1);

            BusinessCollectionBase retList = this.SubCollection(start, size);
            retList.Block.Size = this._blcok.Size;
            retList.Block.Index = this._blcok.Index;
            retList.Block.Count = this._blcok.Count;
            return retList;
        }

        public BusinessCollectionBase SubCollection(int start, int count)
        {
            BusinessCollectionBase retList = (BusinessCollectionBase) Activator.CreateInstance(this.GetType());
            if (count == 0 || start >= this.InnerList.Count)
                return retList;

            for (int i = start; i < start + count; i++)
            {
                if (i >= base.InnerList.Count)
                {
                    return retList;
                }

                retList.InnerList.Add(this.InnerList[i]);
            }

            return retList;
        }

        public BusinessBase this[int index]
        {
            get { return (BusinessBase) base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}