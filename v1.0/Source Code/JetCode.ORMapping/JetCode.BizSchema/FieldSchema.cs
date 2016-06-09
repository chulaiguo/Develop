using System;
using System.Xml;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class FieldSchema
    {
        private string _name = string.Empty;
        private string _alias = string.Empty;
        private string _dataType = string.Empty;
        private string _size = string.Empty;
        private bool _isPK = false;
        private bool _isNullable = true;
        private bool _isJoined = false;

        private string _tableName = string.Empty;
        private string _tableAlias = string.Empty;

        public FieldSchema(string name)
        {
            this._name = name;
        }

        public static FieldSchema LoadFromSchema(string tableName, ColumnSchema column)
        {
            FieldSchema schema = new FieldSchema(column.Name);
            schema.Alias = column.Name;
            schema.DataType = column.DataType;
            schema.Size = column.Size;
            schema.IsPK = column.IsPK;
            schema.IsNullable = column.IsNullable;
            schema.IsJoined = false;

            schema.TableName = tableName;
            schema.TableAlias = tableName;

            return schema;
        }

        public static FieldSchema LoadFromXmlReader(XmlTextReader tr)
        {
            FieldSchema schema = new FieldSchema(tr.GetAttribute("Name"));
            schema.Alias = tr.GetAttribute("Alias");
            schema.DataType = tr.GetAttribute("Type");
            schema.Size = tr.GetAttribute("Size");
            schema.IsPK = Convert.ToBoolean(tr.GetAttribute("IsPK"));
            schema.IsNullable = Convert.ToBoolean(tr.GetAttribute("IsNullable"));
            schema.IsJoined = Convert.ToBoolean(tr.GetAttribute("IsJoined"));
            schema.TableName = tr.GetAttribute("TableName");
            schema.TableAlias = tr.GetAttribute("TableAlias");

            return schema;
        }

        public XmlElement ToXmlElement(XmlDocument doc)
        {
            XmlElement xmlElement = doc.CreateElement("Column");
            xmlElement.SetAttribute("Name", this.Name);
            xmlElement.SetAttribute("Alias", this.Alias);
            xmlElement.SetAttribute("Type", this.DataType);
            xmlElement.SetAttribute("Size", this.Size);
            xmlElement.SetAttribute("IsPK", this.IsPK.ToString());
            xmlElement.SetAttribute("IsNullable", this.IsNullable.ToString());
            xmlElement.SetAttribute("IsJoined", this.IsJoined.ToString());
            xmlElement.SetAttribute("TableName", this.TableName);
            xmlElement.SetAttribute("TableAlias", this.TableAlias);
            return xmlElement;
        }

        public FieldSchema Clone()
        {
            FieldSchema clone = new FieldSchema(this.Name);
            clone.Alias = this.Name;
            clone.DataType = this.DataType;
            clone.Size = this.Size;
            clone.IsPK = this.IsPK;
            clone.IsNullable = this.IsNullable;
            clone.IsJoined = this.IsJoined;

            clone.TableName = this.TableName;
            clone.TableAlias = this.TableAlias;

            return clone;
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

        public string Name
        {
            get { return this._name; }
        }

        public string Alias
        {
            get { return this._alias.Replace(" ", "_"); }
            set { this._alias = value; }
        }

        public bool IsJoined
        {
            get { return this._isJoined; }
            set { this._isJoined = value; }
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

        public string Size
        {
            get { return this._size; }
            set { this._size = value; }
        }

        public string TableName
        {
            get { return this._tableName; }
            set { this._tableName = value; }
        }

        public string TableAlias
        {
            get { return this._tableAlias; }
            set { this._tableAlias = value; }
        }
    }
}