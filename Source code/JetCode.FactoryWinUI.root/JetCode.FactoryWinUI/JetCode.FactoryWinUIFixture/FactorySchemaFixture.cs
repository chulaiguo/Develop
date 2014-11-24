using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactorySchemaFixture : FixtureBase
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        protected override string SrcDirectory
        {
            get { return string.Format(@"{0}\Misc.SchemaCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDBSchema detail = new FactoryDBSchema(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string viewFileName = string.Format(@"{0}\DBSchema.cs", this.SrcDirectory);

            FactoryDBSchema view = new FactoryDBSchema(base.Schema);
            base.WriteToFile(viewFileName, view);
        }
    }
}
