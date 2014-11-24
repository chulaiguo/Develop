using System;

namespace JetCode.BusinessEntity
{
    [Serializable]
    public class CollectionBlock
    {
        private int _count = -1;
        private int _index = -1;
        private int _size = -1;

        public int Count
        {
            get { return this._count; }
            set { this._count = value; }
        }

        public int Index
        {
            get { return this._index; }
            set { this._index = value; }
        }

        public int Size
        {
            get { return this._size; }
            set { this._size = value; }
        }
    }
}