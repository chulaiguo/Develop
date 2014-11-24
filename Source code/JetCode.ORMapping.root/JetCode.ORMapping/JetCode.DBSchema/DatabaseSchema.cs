namespace JetCode.DBSchema
{
    public class DatabaseSchema
    {
        private string _name;
        private TableSchemaCollection _tables;

        public DatabaseSchema(string name)
        {
            this._name = name;
            this._tables = new TableSchemaCollection();
        }

        public override string ToString()
        {
            return this._name;
        }


        public string Name
        {
            get { return this._name; }
        }

        public TableSchemaCollection Tables
        {
            get { return this._tables; }
        }
    }
}