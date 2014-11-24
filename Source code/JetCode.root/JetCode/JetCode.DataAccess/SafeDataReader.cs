using System;
using System.Data;

namespace JetCode.DataAccess
{
    public class SafeDataReader : IDataReader
    {
        private readonly IDataReader _dataReader = null;

        public SafeDataReader(IDataReader dataReader)
        {
            this._dataReader = dataReader;
        }

        public void Close()
        {
            this._dataReader.Close();
        }

        public void Dispose()
        {
            this._dataReader.Dispose();
        }

        public bool GetBoolean(int i)
        {
            if (this.IsDBNull(i))
            {
                return false;
            }
            return this._dataReader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            if (this.IsDBNull(i))
            {
                return byte.MinValue;
            }
            return this._dataReader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public char GetChar(int i)
        {
            if (this.IsDBNull(i))
            {
                return char.MinValue;
            }
            return this._dataReader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public IDataReader GetData(int i)
        {
            return null;
        }

        public string GetDataTypeName(int i)
        {
            return this._dataReader.GetFieldType(i).FullName;
        }

        public DateTime GetDateTime(int i)
        {
            if (this.IsDBNull(i))
            {
                return DateTime.MinValue;
            }
            return this._dataReader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            if (this.IsDBNull(i))
            {
                return decimal.MinValue;
            }
            return this._dataReader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            if (this.IsDBNull(i))
            {
                return double.MinValue;
            }
            return this._dataReader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return this._dataReader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            if (this.IsDBNull(i))
            {
                return float.MinValue;
            }
            return this._dataReader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            if (this.IsDBNull(i))
            {
                return Guid.Empty;
            }
            return this._dataReader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            if (this.IsDBNull(i))
            {
                return short.MinValue;
            }
            return this._dataReader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            if (this.IsDBNull(i))
            {
                return int.MinValue;
            }
            return this._dataReader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            if (this.IsDBNull(i))
            {
                return long.MinValue;
            }
            return this._dataReader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return null;
        }

        public int GetOrdinal(string name)
        {
            return this._dataReader.GetOrdinal(name);
        }

        public DataTable GetSchemaTable()
        {
            return this._dataReader.GetSchemaTable();
        }

        public string GetString(int i)
        {
            if (this.IsDBNull(i))
            {
                return string.Empty;
            }
            return this._dataReader.GetString(i);
        }

        public object GetValue(int i)
        {
            return null;
        }

        public int GetValues(object[] values)
        {
            return 0;
        }

        public bool IsDBNull(int i)
        {
            return this._dataReader.IsDBNull(i);
        }

        public bool NextResult()
        {
            return this._dataReader.NextResult();
        }

        public bool Read()
        {
            return this._dataReader.Read();
        }

        public int Depth
        {
            get { return this._dataReader.Depth; }
        }

        public int FieldCount
        {
            get { return this._dataReader.FieldCount; }
        }

        public bool IsClosed
        {
            get { return this._dataReader.IsClosed; }
        }

        public object this[string name]
        {
            get
            {
                int ordinal = this._dataReader.GetOrdinal(name);
                if (this.IsDBNull(ordinal))
                {
                    return this.GetEmptyValue(this.GetDataTypeName(ordinal));
                }
                return this._dataReader[name];
            }
        }

        public int RecordsAffected
        {
            get { return this._dataReader.RecordsAffected; }
        }

        object IDataRecord.this[int index]
        {
            get
            {
                if (this.IsDBNull(index))
                {
                    return this.GetEmptyValue(this.GetDataTypeName(index));
                }
                return this._dataReader[index];
            }
        }

        private object GetEmptyValue(string typeName)
        {
            switch (typeName)
            {
                case "System.Boolean":
                    return false;

                case "System.Byte":
                    return (byte)0;

                case "System.Char":
                    return '\0';

                case "System.Decimal":
                    return 0.0M;

                case "System.Double":
                    return 0.0;

                case "System.Single":
                    return 0f;

                case "System.Int16":
                    return (short)0;

                case "System.Int32":
                    return 0;

                case "System.Int64":
                    return 0L;

                case "System.String":
                    return string.Empty;

                case "System.DateTime":
                    //return new DateTime(1900, 1, 1);
                    return DateTime.Now;

                case "System.Guid":
                    return Guid.Empty;
            }
            return null;
        }
    }
}