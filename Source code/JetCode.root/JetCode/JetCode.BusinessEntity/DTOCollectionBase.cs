using System;
using System.Collections;

namespace JetCode.BusinessEntity
{
    [Serializable]
    public abstract class DTOCollectionBase : CollectionBase
    {
        private readonly CollectionBlock _blcok = new CollectionBlock();
        public CollectionBlock Block
        {
            get { return this._blcok; }
        }

        public DTOBase this[int index]
        {
            get { return (DTOBase)base.List[index]; }
            set { base.List[index] = value; }
        }

        public DTOCollectionBase SubCollection()
        {
            int start = this._blcok.Index * this._blcok.Size;
            int size = this._blcok.Size;
            this._blcok.Count = (this.InnerList.Count / this._blcok.Size) + (((this.InnerList.Count % this._blcok.Size) == 0) ? 0 : 1);

            DTOCollectionBase retList = this.SubCollection(start, size);
            retList.Block.Size = this._blcok.Size;
            retList.Block.Index = this._blcok.Index;
            retList.Block.Count = this._blcok.Count;
            return retList;
        }

        public DTOCollectionBase SubCollection(int start, int count)
        {
            DTOCollectionBase retList = (DTOCollectionBase)Activator.CreateInstance(this.GetType());
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
    }
}