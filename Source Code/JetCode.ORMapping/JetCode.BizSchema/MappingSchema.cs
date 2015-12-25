using System.IO;
using System.Text;
using System.Xml;
using JetCode.DBSchema;

namespace JetCode.BizSchema
{
    public class MappingSchema
    {
        private string _name = string.Empty;
        private ObjectSchemaCollection _objects;

        public MappingSchema(DatabaseSchema dbSchema)
        {
            this._objects = new ObjectSchemaCollection();

            this.LoadFromSchema(dbSchema);
        }

        public MappingSchema(string xmlFile)
        {
            this._objects = new ObjectSchemaCollection();

            this.LoadFromFile(xmlFile);
        }

        private void LoadFromSchema(DatabaseSchema dbSchema)
        {
            this._name = dbSchema.Name;

            foreach (TableSchema item in dbSchema.Tables)
            {
                ObjectSchema schema = ObjectSchema.LoadFromSchema(dbSchema.Name, item);
                this.Objects.Add(schema);
            }
        }

        private void LoadFromFile(string file)
        {
            XmlTextReader reader = new XmlTextReader(file);
            while (reader.Read())
            {
                if(reader.NodeType != XmlNodeType.Element)
                    continue;

                if (string.Compare(reader.Name, "Mapping", true) == 0)
                {
                    this.Name = reader.GetAttribute("Name");
                    continue;
                }

                if (string.Compare(reader.Name, "Table", true) == 0)
                {
                    ObjectSchema schema = ObjectSchema.LoadFromXmlReader(reader);
                    this.Objects.Add(schema);
                }
            }

            reader.Close();
        }

        public XmlDocument ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(declaration);

            XmlElement xmlDatabase = doc.CreateElement("Mapping");
            xmlDatabase.SetAttribute("Name", this.Name);
            doc.AppendChild(xmlDatabase);

            foreach (ObjectSchema item in this.Objects)
            {
                xmlDatabase.AppendChild(item.ToXml(doc));
            }

            return doc;
        }



        public void SaveToFile(string fileName)
        {
            this.ToXml().Save(fileName);
        }

        public ObjectSchema GetObjectByName(string name)
        {
            foreach (ObjectSchema item in this.Objects)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public void GetParentList(ObjectSchema objSchema, ParentSchemaCollection list)
        {
            foreach (ParentSchema parent in objSchema.Parents)
            {
                if (list.Find(parent.Name) == null)
                {
                    list.Add(parent);
                }

                ObjectSchema item = this.GetObjectByName(parent.Name);
                if (item == null)
                    continue;

                this.GetParentList(item, list);
            }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public ObjectSchemaCollection Objects
        {
            get { return this._objects; }
        }
    }
}