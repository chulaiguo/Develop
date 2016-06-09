using JetCode.DBSchema;
using JetCode.DBSchema.Factory;

namespace JetCode.BizSchema.Factory
{
    public static class SchemaFactory
    {
        public static MappingSchema GetMappingSchema(string connString, string database)
        {
            MSSQLSchemaFactory factory = new MSSQLSchemaFactory(database, connString);
            DatabaseSchema schema = factory.LoadDatabaseSchema();
            return GetMappingSchema(schema);
        }

        internal static DatabaseSchema GetDatabaseSchema(string connString, string database)
        {
            MSSQLSchemaFactory factory = new MSSQLSchemaFactory(database, connString);
            return factory.LoadDatabaseSchema();
        }

        public static MappingSchema GetMappingSchema(DatabaseSchema dbSchema)
        {
            return new MappingSchema(dbSchema);
        }

        public static MappingSchema GetMappingSchema(string xmlFile)
        {
            return new MappingSchema(xmlFile);
        }
    }
}