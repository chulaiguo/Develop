using System.IO;
using JetCode.Factory;
using JetCode.BizSchema;

namespace JetCode.FactoryWinUI
{
    public class FactoryLoginService : FactoryBase
    {
        public FactoryLoginService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.WinCtrl.Login;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class LoginService : ILogin");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic Result Login(string userId, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResult r = new Result(true);");
            writer.WriteLine("\t\t\tr.Tag = 30;");
            writer.WriteLine("\t\t\treturn r;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic Result ChangePassword(string userId, string oldPassword, string newPassword)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic Result RecoverPassword(string userId)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic int MaxTryCount");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn 3;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
