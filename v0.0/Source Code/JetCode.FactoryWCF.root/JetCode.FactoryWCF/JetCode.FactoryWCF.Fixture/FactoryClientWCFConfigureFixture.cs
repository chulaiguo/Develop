using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryClientWCFConfigureFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.ClientConfigure", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryClientWCFConfigure detail = new FactoryClientWCFConfigure(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ServiceReferences.ClientConfig", this.SrcDirectory);

            FactoryClientWCFConfigure entity = new FactoryClientWCFConfigure(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}