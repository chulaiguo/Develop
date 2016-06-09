namespace JetCode.DBSchema
{
    public class ColumnSchema
    {
        private string _name;
        private string _dataType;
        private string _size;
        private bool _isPK;
        private bool _isNullable;

        public ColumnSchema(string name)
        {
            this._name = name;
            this._dataType = string.Empty;
            this._size = string.Empty;
            this._isNullable = true;
        }

        public override string ToString()
        {
            return this._name;
        }

        public string DataType
        {
            get { return this._dataType; }
            set { this._dataType = value; }
        }

        public bool IsNullable
        {
            get { return this._isNullable; }
            set { this._isNullable = value; }
        }

        public bool IsPK
        {
            get { return this._isPK; }
            set { this._isPK = value; }
        }

        public string Name
        {
            get { return this._name; }
        }

        public string Size
        {
            get { return this._size; }
            set { this._size = value; }
        }
    }
}