using System.IO;
using System.Text;

namespace JetCode.BizSchema
{
    public class MappingCheckErrors
    {
        private MappingSchema _mapingSchema = null;

        public MappingCheckErrors(MappingSchema mappingSchema)
        {
            this._mapingSchema = mappingSchema;
        }

        private ObjectSchemaCollection Objects
        {
            get { return this._mapingSchema.Objects; }
        }

        public string CheckErrors()
        {
            StringWriter writer = new StringWriter(new StringBuilder());

            foreach (ObjectSchema item in this.Objects)
            {
                string error = this.CheckObjectErrors(item);
                if (error.Length > 0)
                {
                    writer.WriteLine("== {0} Joined Fields ==", item.Name);
                    writer.WriteLine(error);
                    writer.WriteLine();
                }
            }

            return writer.ToString();
        }

        private string CheckObjectErrors(ObjectSchema objectSchema)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FieldSchema item in objectSchema.Fields)
            {
                if (!item.IsJoined)
                    continue;

                if (item.IsPK)
                {
                    builder.AppendFormat(string.Format("\t{0} should't be primary key.", item.Name));
                    continue;
                }

                JoinSchema join = objectSchema.Joins.Find(item.TableAlias);
                if (join == null)
                {
                    builder.AppendLine(string.Format("\t{0} does't exist.", item.TableAlias));
                    continue;
                }

                ObjectSchema refObject = this.Objects.Find(join.RefTable);
                if (refObject == null)
                {
                    builder.AppendLine(string.Format("\t{0}(Alias:{1}) does't exist.", join.RefTable, item.TableAlias));
                    continue;
                }

                FieldSchema field = refObject.Fields.Find(item.Name);
                if (field == null)
                {
                    builder.AppendLine(string.Format("\t{0} does't exist in {1}.", item.Name, refObject));
                    continue;
                }

                if (item.DataType != field.DataType)
                    builder.AppendLine(
                        string.Format("\tDataType should be {0}, not {1}.", field.DataType, item.DataType));
                if (item.Size != field.Size)
                    builder.AppendLine(string.Format("\tSize should be {0}, not {1}.", field.Size, item.Size));
                if (item.IsNullable != field.IsNullable)
                    builder.AppendLine(
                        string.Format("\tIsNull should be {0}, not {1}.", field.IsNullable, item.IsNullable));
            }

            return builder.ToString();
        }
    }
}