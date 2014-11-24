namespace JetCode.DBSchema
{
    public class TableSchema
    {
        private string _name;
        private ColumnSchemaCollection _columns;
        private RelationshipSchemaCollection _relationships;
        private DBIndexSchemaCollection _indexs;

        public TableSchema(string name)
        {
            this._name = name;
            this._columns = new ColumnSchemaCollection();
            this._relationships = new RelationshipSchemaCollection();
            this._indexs = new DBIndexSchemaCollection();
        }

        public override string ToString()
        {
            return this._name;
        }

        public ColumnSchemaCollection Columns
        {
            get { return this._columns; }
        }

        public RelationshipSchemaCollection Relationship
        {
            get { return this._relationships; }
        }

        public string Name
        {
            get { return this._name; }
        }

        public DBIndexSchemaCollection Indexs
        {
            get { return _indexs; }
        }
    }
}