using System.Xml;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class JoinSchema
    {
        private string _keyTable;
        private string _keyAlias;
        private string _refTable;
        private string _refAlias;
        private string _keyField;
        private string _refField;

        public static JoinSchema LoadFromSchema(RelationshipSchema relationship)
        {
            JoinSchema schema = new JoinSchema();
            schema.KeyTable = relationship.PKTableName;
            schema.KeyAlias = relationship.PKTableName;
            schema.RefTable = relationship.FKTableName;
            schema.RefAlias = relationship.FKTableName;

            schema.KeyField = relationship.LocalColumnName;
            schema.RefField = relationship.RemoteColumnName;

            return schema;
        }

        public static JoinSchema LoadFromXmlReader(XmlTextReader tr)
        {
            JoinSchema schema = new JoinSchema();
            schema.KeyTable = tr.GetAttribute("KeyTable");
            schema.KeyAlias = tr.GetAttribute("KeyAlias");
            schema.RefTable = tr.GetAttribute("RefTable");
            schema.RefAlias = tr.GetAttribute("RefAlias");
            schema.KeyField = tr.GetAttribute("KeyField");
            schema.RefField = tr.GetAttribute("RefField");

            return schema;
        }


        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xmlElement = doc.CreateElement("Join");
            xmlElement.SetAttribute("KeyTable", this.KeyTable);
            xmlElement.SetAttribute("KeyAlias", this.KeyAlias);
            xmlElement.SetAttribute("RefTable", this.RefTable);
            xmlElement.SetAttribute("RefAlias", this.RefAlias);
            xmlElement.SetAttribute("KeyField", this.KeyField);
            xmlElement.SetAttribute("RefField", this.RefField);

            return xmlElement;
        }

        public JoinSchema Clone()
        {
            JoinSchema clone = new JoinSchema();
            clone.KeyTable = this.KeyTable;
            clone.KeyAlias = this.KeyAlias;
            clone.RefTable = this.RefTable;
            clone.RefAlias = this.RefAlias;

            clone.KeyField = this.KeyField;
            clone.RefField = this.RefField;

            return clone;
        }
     
        public override string ToString()
        {
            return this.KeyAlias + this.RefAlias;
        } 
        
        public string JoinCommand
        {
            get
            {
                return string.Format("[{0}].[{1}] = [{2}].[{3}]", this.KeyAlias, this.KeyField,
                                     this.RefAlias, this.RefField);
            }
        }

        public string KeyAlias
        {
            get { return this._keyAlias.Replace(" ", "_"); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._keyAlias = this._keyTable;
                }
                else
                {
                    this._keyAlias = value.Trim();
                }
            }
        }

        public string KeyTable
        {
            get { return this._keyTable; }
            set { this._keyTable = value; }
        }

        public string RefAlias
        {
            get { return this._refAlias.Replace(" ", "_"); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._refAlias = this._refTable;
                }
                else
                {
                    this._refAlias = value.Trim();
                }
            }
        }

        public string RefTable
        {
            get { return this._refTable; }
            set { this._refTable = value; }
        }

        public string KeyField
        {
            get { return this._keyField; }
            set { this._keyField = value; }
        }

        public string RefField
        {
            get { return this._refField; }
            set { this._refField = value; }
        }
    }
}