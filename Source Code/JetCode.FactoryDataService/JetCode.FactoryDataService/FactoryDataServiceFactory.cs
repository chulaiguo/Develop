using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryDataServiceFactory : FactoryBase
    {
        public FactoryDataServiceFactory(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Configuration;");
            writer.WriteLine("using System.Security.Authentication;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this.ProjectName);
            writer.WriteLine("using {0}.DataServiceBase;", this.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic partial class {0}DataService : {0}DataServiceBase", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tpublic {0}DataService(string connectionString, SecurityToken token)", item.Alias);
                writer.WriteLine("\t\t\t: base(connectionString, token)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\t#region Override Children Services");
                foreach (ChildSchema child in item.Children)
                {
                    writer.WriteLine("\t\tprotected override {0}DataServiceBase Get{0}DataServiceBase(SecurityToken token)",
                                     child.Alias);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn DataServiceFactory.Create{0}DataService(token, true);", child.Alias);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine("\t}");
            }

            writer.WriteLine();
            this.WriteDataServiceFactory(writer);
        }

        private void WriteDataServiceFactory(StringWriter writer)
        {
            writer.WriteLine("\tinternal static class DataServiceFactory");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate static readonly string _ConnectionString = ConfigurationManager.AppSettings[\"DB:{0}\"];", base.ProjectName);
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static string ConnectionString");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _ConnectionString; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static void CheckAuthorize(SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(token.IsAnonymous || Utils.Authentication.IsAuthenticated(token))");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthrow new AuthenticationException(string.Format(\"The UserId/Password(UserID={0}) is not valid\", token.UserId));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t\tpublic static {0}DataService Create{0}DataService(SecurityToken token, bool isUsedByOtherService)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (isUsedByOtherService)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\ttoken = SecurityToken.CreateDuplicateToken(token, false);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\treturn new {0}DataService(_ConnectionString, token);", item.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t}");
        }
    }
}
