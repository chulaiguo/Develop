using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFBasicPublish : FactoryBase
    {
        public FactoryWCFBasicPublish(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
        }

        protected override void EndWrite(StringWriter writer)
        {
        }

        protected override void WriteContent(StringWriter writer)
        {
           writer.WriteLine("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"{0}.WCFService.{1}WCFService\" %>", base.ProjectName, base.ObjectSchema.Alias);
        }
    }
}
