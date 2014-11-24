namespace JetCode.DBSchema
{
    public class RelationshipSchema
    {
        private string _name;
        private bool _isParent;
        private string _localColumn;
        private string _remoteColumn;

        private string _PKTableName;
        private string _FKTableName;

        public RelationshipSchema(string name)
        {
            this._name = name;
            this._isParent = false;
            
            this._localColumn = string.Empty;
            this._remoteColumn = string.Empty;

            this._PKTableName = string.Empty;
            this._FKTableName = string.Empty;
        }

        public override string ToString()
        {
            return this._name;
        }

        public string Name
        {
            get { return this._name; }
        }

        public bool IsParent
        {
            get { return this._isParent; }
            set { this._isParent = value; }
        }

        public string LocalColumnName
        {
            get { return this._localColumn; }
            set { this._localColumn = value; }
        }

        public string RemoteColumnName
        {
            get { return this._remoteColumn; }
            set { this._remoteColumn = value; }
        }

        public string PKTableName
        {
            get { return this._PKTableName; }
            set { this._PKTableName = value; }
        }

        public string FKTableName
        {
            get { return this._FKTableName; }
            set { this._FKTableName = value; }
        }
    }
}