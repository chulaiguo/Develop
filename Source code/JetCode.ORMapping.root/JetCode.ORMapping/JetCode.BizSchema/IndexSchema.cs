using System.Collections.Specialized;
using System.Text;
using System.Xml;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class IndexSchema
    {
        private string _name = string.Empty;
        private StringCollection _keys = null;
        private bool _isPrimaryKey = false;
        private bool _isUniqueConstraint = false;

        public IndexSchema(string name)
        {
            this._name = name;

            this._keys = new StringCollection();
        }

        public static IndexSchema LoadFromSchema(DBIndexSchema dbIndex)
        {
            IndexSchema schema = new IndexSchema(dbIndex.Name);
            foreach (string key in dbIndex.Keys)
            {
                schema.Keys.Add(key);
            }
            schema.IsPrimaryKey = dbIndex.IsPrimaryKey;
            schema.IsUniqueConstraint = dbIndex.IsUniqueConstraint;

            return schema;
        }

        public static IndexSchema LoadFromXmlReader(XmlTextReader tr)
        {
            IndexSchema schema = new IndexSchema(tr.GetAttribute("Name"));
            string[] keys = tr.GetAttribute("Keys").Split(',');
            schema.Keys.AddRange(keys);
            schema.IsPrimaryKey = bool.Parse(tr.GetAttribute("IsPrimaryKey"));
            schema.IsUniqueConstraint = bool.Parse(tr.GetAttribute("IsUniqueConstraint"));

            return schema;
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xmlElement = doc.CreateElement("Index");
            xmlElement.SetAttribute("Name", this.Name);
            xmlElement.SetAttribute("Keys", this.KeysString);
            xmlElement.SetAttribute("IsPrimaryKey", this.IsPrimaryKey.ToString());
            xmlElement.SetAttribute("IsUniqueConstraint", this.IsUniqueConstraint.ToString());
            return xmlElement;
        }

        public IndexSchema Clone()
        {
            IndexSchema clone = new IndexSchema(this.Name);
            foreach (string key in this.Keys)
            {
                clone.Keys.Add(key);
            }
            clone.IsPrimaryKey = this.IsPrimaryKey;
            clone.IsUniqueConstraint = this.IsUniqueConstraint;

            return clone;
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

        public string KeysString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (string item in this.Keys)
                {
                    builder.AppendFormat("{0},", item);
                }

                return builder.ToString().TrimEnd(',');
            }
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