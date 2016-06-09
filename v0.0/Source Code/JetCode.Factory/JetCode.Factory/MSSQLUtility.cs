using System;

namespace JetCode.Factory
{
    public class MSSQLUtility : UtilityBase
    {
        public override Type ToDotNetType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "tinyint":
                    return typeof(byte);
                case "smallint":
                    return typeof(short);
                case "int":
                    return typeof(int);
                case "bigint":
                    return typeof(long);
                case "bit":
                    return typeof(bool);
                case "decimal":
                case "numeric":
                case "smallmoney":
                case "money":
                    return typeof(decimal);
                case "float":
                    return typeof(float);
                case "real":
                    return typeof(double);
                case "smalldatetime":
                case "datetime":
                    return typeof(DateTime);
                case "char":
                case "varchar":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                    return typeof(string);
                case "uniqueidentifier":
                    return typeof(Guid);
                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                case "sql_variant":
                    return typeof(byte[]);
                default:
                    return typeof(object);
            }
        }
    }
}