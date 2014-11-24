namespace JetCode.DataServiceBase
{
    public class TablePermission
    {
        private readonly bool _deletable = false;
        private readonly bool _insertable = false;
        private readonly bool _selectable = false;
        private readonly bool _updatable = false;

        public TablePermission(bool insertable, bool selectable, bool updatable, bool deletable)
        {
            this._insertable = insertable;
            this._selectable = selectable;
            this._updatable = updatable;
            this._deletable = deletable;
        }

        public bool Deletable
        {
            get { return this._deletable; }
        }

        public bool Insertable
        {
            get { return this._insertable; }
        }

        public bool Selectable
        {
            get { return this._selectable; }
        }

        public bool Updatable
        {
            get { return this._updatable; }
        }
    }
}