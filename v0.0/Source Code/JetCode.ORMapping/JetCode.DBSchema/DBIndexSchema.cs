using System.Collections.Specialized;

namespace JetCode.DBSchema
{
    public class DBIndexSchema
    {
        private string _name = string.Empty;
        private StringCollection _keys = null;
        private bool _isPrimaryKey = false;
        private bool _isUniqueConstraint = false;

        public DBIndexSchema(string name)
        {
            this._name = name;

            this._keys = new StringCollection();
        }

        public override string ToString()
        {
            return this._name;
        }

        public string Name
        {
            get { return _name; }
        }

        public StringCollection Keys
        {
            get { return _keys; }
        }

        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
            set { _isPrimaryKey = value; }
        }

        public bool IsUniqueConstraint
        {
            get { return _isUniqueConstraint; }
            set { _isUniqueConstraint = value; }
        }
    }
}