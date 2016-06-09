using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDTOCommon : FactoryBase
    {
        public FactoryDTOCommon(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.Serialization;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DTO", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteSecurityToken(writer);
            this.WriteResult(writer);
        }

        private void WriteSecurityToken(StringWriter writer)
        {
            writer.WriteLine("\t[DataContract]");
            writer.WriteLine("\tpublic class TokenDTO");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate Guid _tokenID = Guid.NewGuid();");
            writer.WriteLine("\t\tprivate int _blockIndex = -1;");
            writer.WriteLine("\t\tprivate int _blockSize = 1000;");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate string _userid = string.Empty;");
            writer.WriteLine("\t\tprivate string _password = string.Empty;");
            writer.WriteLine("\t\tprivate string _ticks = string.Empty;");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic TokenDTO(string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._userid = userid;");
            writer.WriteLine("\t\t\tthis._ticks = DateTime.Now.Ticks.ToString();");
            writer.WriteLine("\t\t\tthis._password = this.HashValue(password);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate string HashValue(string value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn string.Format(\"{0}{1}\", value, this._ticks).GetHashCode().ToString();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t[DataMember]");
            writer.WriteLine("\t\tpublic string UserId");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._userid; }");
            writer.WriteLine("\t\tset { this._userid = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t[DataMember]");
            writer.WriteLine("\t\tpublic string Password");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._password; }");
            writer.WriteLine("\t\tset { this._password = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t[DataMember]");
            writer.WriteLine("\t\tpublic string Ticks");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._ticks; }");
            writer.WriteLine("\t\tset { this._ticks = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t[DataMember]");
            writer.WriteLine("\t\tpublic Guid TokenID");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._tokenID; }");
            writer.WriteLine("\t\t\tset { this._tokenID = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t[DataMember]");
            writer.WriteLine("\t\tpublic int BlockIndex");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._blockIndex; }");
            writer.WriteLine("\t\t\tset { this._blockIndex = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t[DataMember]");
            writer.WriteLine("\t\tpublic int BlockSize");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._blockSize; }");
            writer.WriteLine("\t\t\tset { this._blockSize = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteResult(StringWriter writer)
        {
            writer.WriteLine("\t[DataContract]");
            writer.WriteLine("\tpublic class ResultDTO");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate Dictionary<Guid, byte[]> _rowversionList = new Dictionary<Guid,byte[]>();");
            writer.WriteLine("\t\tprivate Dictionary<Guid, string> _errorList = new Dictionary<Guid, string>();");
            writer.WriteLine("\t\tprivate Dictionary<string, string> _tagList = new Dictionary<string, string>();");
            writer.WriteLine();
            writer.WriteLine("\t\t[DataMember]");
            writer.WriteLine("\t\tpublic Dictionary<Guid, byte[]> RowversionList");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._rowversionList;}");
            writer.WriteLine("\t\t\tset { this._rowversionList = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t[DataMember]");
            writer.WriteLine("\t\tpublic Dictionary<Guid, string> ErrorList");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._errorList; }");
            writer.WriteLine("\t\t\tset { this._errorList = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t[DataMember]");
            writer.WriteLine("\t\tpublic Dictionary<string, string> TagList");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._tagList; }");
            writer.WriteLine("\t\t\tset { this._tagList = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
