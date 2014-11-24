using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFBasicConfigureFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.BasicConfigure", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFBasicConfigure detail = new FactoryWCFBasicConfigure(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Web.config", this.SrcDirectory);

            FactoryWCFBasicConfigure entity = new FactoryWCFBasicConfigure(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}