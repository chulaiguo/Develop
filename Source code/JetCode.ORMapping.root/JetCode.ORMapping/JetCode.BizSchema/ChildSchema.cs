using System.Xml;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class ChildSchema
    {
        private string _name = string.Empty;
        private string _alias = string.Empty;
        private string _localColumn = string.Empty;
        private string _remoteColumn = string.Empty;
        private bool _cascadeDelete = true;

        public ChildSchema(string name)
        {
            this._name = name;
        }

        public static ChildSchema LoadFromSchema(RelationshipSchema relationship)
        {
            ChildSchema schema = new ChildSchema(relationship.FKTableName);
            schema.Alias = relationship.FKTableName;
            schema.LocalColumn = relationship.LocalColumnName;
            schema.RemoteColumn = relationship.RemoteColumnName;

            return schema;
        }

        public static ChildSchema LoadFromXmlReader(XmlTextReader tr)
        {
            ChildSchema schema = new ChildSchema(tr.GetAttribute("Name"));
            schema.Alias = tr.GetAttribute("Alias");
            schema.LocalColumn = tr.GetAttribute("LocalColumn");
            schema.RemoteColumn = tr.GetAttribute("RemoteColumn");
            schema.CascadeDelete = bool.Parse(tr.GetAttribute("CascadeDelete"));

            return schema;
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xmlElement = doc.CreateElement("Child");
            xmlElement.SetAttribute("Name", this.Name);
            xmlElement.SetAttribute("Alias", this.Alias);
            xmlElement.SetAttribute("LocalColumn", this.LocalColumn);
            xmlElement.SetAttribute("RemoteColumn", this.RemoteColumn);
            xmlElement.SetAttribute("CascadeDelete", this.CascadeDelete.ToString());
            return xmlElement;
        }

        public ChildSchema Clone()
        {
            ChildSchema clone = new ChildSchema(this.Name);
            clone.Alias = this.Alias;
            clone.LocalColumn = this.LocalColumn;
            clone.RemoteColumn = this.RemoteColumn;

            return clone;
        }

        public override string ToString()
        {
            return string.Format("{0}", this.Name);
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

        public string LocalColumn
        {
            get { return this._localColumn; }
            set { this._localColumn = value; }
        }

        public string RemoteColumn
        {
            get { return this._remoteColumn; }
            set { this._remoteColumn = value; }
        }

        public bool CascadeDelete
        {
            get { return _cascadeDelete; }
            set { _cascadeDelete = value; }
        }
    }
}