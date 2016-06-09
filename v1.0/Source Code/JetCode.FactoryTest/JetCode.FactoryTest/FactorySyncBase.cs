using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactorySyncBase : FactoryBase
    {
        public FactorySyncBase(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.SyncService.Core", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class SyncBase");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate readonly SecurityToken _token = null;");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected SyncBase(SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._token = token;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected SecurityToken Token");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _token; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteDebugMethodBegin()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteDebugMethodBegin();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteDebugMethodEnd()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteDebugMethodEnd();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteDebug(string message)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteDebug(message);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteError(string error)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteError(error);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteDeleteLog(BusinessBase entity, Result result)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.WriteDebug(result.ToString());");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteSaveLog(BusinessBase entity, Result result)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.WriteDebug(result.ToString());");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteSaveCommandLog(Result result)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteDebug(result.ToString());");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected void WriteSaveCacheLog(Result result)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteDebug(result.ToString());");
            writer.WriteLine("\t\t}");          
        }
    }
}
