using System.Collections.Generic;
using System.Xml;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class ObjectSchema
    {     
        private string _name = string.Empty;
        private string _alias = string.Empty;
        private string _dbName = string.Empty;  

        private ParentSchemaCollection _parents;
        private ChildSchemaCollection _childs;
        private FieldSchemaCollection _fields;
        private JoinSchemaCollection _joins;
        private IndexSchemaCollection _indexs;

        public ObjectSchema(string name)
        {
            this._name = name;

            this._fields = new FieldSchemaCollection();
            this._joins = new JoinSchemaCollection();
            this._childs = new ChildSchemaCollection();
            this._parents = new ParentSchemaCollection();
            this._indexs = new IndexSchemaCollection();
        }

        public static ObjectSchema LoadFromSchema(string dbName, TableSchema table)
        {
            ObjectSchema schema = new ObjectSchema(table.Name);
            schema.Alias = table.Name;
            schema.DBName = dbName;

            foreach (ColumnSchema item in table.Columns)
            {
                FieldSchema field = FieldSchema.LoadFromSchema(table.Name, item);
                schema.Fields.Add(field);
            }

            foreach (RelationshipSchema item in table.Relationship)
            {
                if (item.IsParent)
                {
                    JoinSchema join = JoinSchema.LoadFromSchema(item);
                    schema.Joins.Add(join);

                    ParentSchema parent = ParentSchema.LoadFromSchema(item);
                    schema.Parents.Add(parent);
                }
                else
                {
                    ChildSchema child = ChildSchema.LoadFromSchema(item);
                    schema.Children.Add(child);
                }
            }

            foreach (DBIndexSchema item in table.Indexs)
            {
                IndexSchema index = IndexSchema.LoadFromSchema(item);
                schema.Indexs.Add(index);
            }

            return schema;
        }

        public static ObjectSchema LoadFromXmlReader(XmlTextReader tr)
        {
            ObjectSchema schema = new ObjectSchema(tr.GetAttribute("Name"));
            schema.Alias = tr.GetAttribute("Alias");
            schema.DBName = tr.GetAttribute("Database");

            while (tr.Read())
            {
                if ((tr.NodeType == XmlNodeType.EndElement) && (string.Compare(tr.Name, "Table", true) == 0))
                {
                    break;
                }

                if (tr.NodeType != XmlNodeType.Element)
                    continue;

                if (string.Compare(tr.Name, "Column", true) == 0)
                {
                    FieldSchema field = FieldSchema.LoadFromXmlReader(tr);
                    schema.Fields.Add(field);
                }
                else if (string.Compare(tr.Name, "Join", true) == 0)
                {
                    JoinSchema join = JoinSchema.LoadFromXmlReader(tr);
                    schema.Joins.Add(join);
                }
                else if (string.Compare(tr.Name, "Child", true) == 0)
                {
                    ChildSchema child = ChildSchema.LoadFromXmlReader(tr);
                    schema.Children.Add(child);
                }
                else if (string.Compare(tr.Name, "Parent", true) == 0)
                {
                    ParentSchema parent = ParentSchema.LoadFromXmlReader(tr);
                    schema.Parents.Add(parent);
                }
                else if (string.Compare(tr.Name, "Index", true) == 0)
                {
                    IndexSchema index = IndexSchema.LoadFromXmlReader(tr);
                    schema.Indexs.Add(index);
                }
            }

            return schema;
        }

        public XmlNode ToXml(XmlDocument doc)
        {
            XmlElement xmlNode = doc.CreateElement("Table");
            xmlNode.SetAttribute("Name", this.Name);
            xmlNode.SetAttribute("Alias", this.Alias);
            xmlNode.SetAttribute("Database", this.DBName);

            XmlElement xmlElement = doc.CreateElement("Columns");
            foreach (FieldSchema item in this.Fields)
            {
                xmlElement.AppendChild(item.ToXmlElement(doc));
            }
            xmlNode.AppendChild(xmlElement);

            xmlElement = doc.CreateElement("Joins");
            foreach (JoinSchema item in this.Joins)
            {
                xmlElement.AppendChild(item.ToXml(doc));
            }
            xmlNode.AppendChild(xmlElement);

            xmlElement = doc.CreateElement("Parents");
            foreach (ParentSchema item in this.Parents)
            {
                xmlElement.AppendChild(item.ToXml(doc));
            }
            xmlNode.AppendChild(xmlElement);

            xmlElement = doc.CreateElement("Children");
            foreach (ChildSchema item in this.Children)
            {
                xmlElement.AppendChild(item.ToXml(doc));
            }
            xmlNode.AppendChild(xmlElement);

            xmlElement = doc.CreateElement("Indexs");
            foreach (IndexSchema item in this.Indexs)
            {
                xmlElement.AppendChild(item.ToXml(doc));
            }
            xmlNode.AppendChild(xmlElement);

            return xmlNode;
        }

        public ObjectSchema Clone()
        {
            ObjectSchema clone = new ObjectSchema(this.Name);
            clone.Alias = this.Alias;
            clone.DBName = this.DBName;

            foreach (FieldSchema item in this.Fields)
            {
                clone.Fields.Add(item.Clone());
            }

            foreach (ChildSchema item in this.Children)
            {
                clone.Children.Add(item.Clone());
            }

            foreach (ParentSchema item in this.Parents)
            {
                clone.Parents.Add(item.Clone());
            }

            foreach (JoinSchema item in this.Joins)
            {
                clone.Joins.Add(item.Clone());
            }

            foreach (IndexSchema item in this.Indexs)
            {
                clone.Indexs.Add(item.Clone());
            }
           
            return clone;
        }

        public override string ToString()
        {
            return this._name;
        }

        public bool IsMultiPK
        {
            get
            {
                List<FieldSchema> list = this.GetPKList();
                return list.Count > 1 ? true : false;
            }
        }

        public List<FieldSchema> GetPKList()
        {
            List<FieldSchema> list = new List<FieldSchema>();

            foreach (FieldSchema item in this.Fields)
            {
                if (item.IsPK)
                    list.Add(item);
            }

            return list;
        }

        public FieldSchema GetRowVersion()
        {
            foreach (FieldSchema item in this.Fields)
            {
                if (string.Compare(item.DataType, "timestamp", true) == 0)
                    return item;
            }

            return null;
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

        public string DBName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        public ChildSchemaCollection Children
        {
            get { return this._childs; }
        }

        public ParentSchemaCollection Parents
        {
            get { return this._parents; }
        }

        public FieldSchemaCollection Fields
        {
            get { return this._fields; }
        }

        public JoinSchemaCollection Joins
        {
            get { return this._joins; }
        }

        public IndexSchemaCollection Indexs
        {
            get { return _indexs; }
        }
    }
}