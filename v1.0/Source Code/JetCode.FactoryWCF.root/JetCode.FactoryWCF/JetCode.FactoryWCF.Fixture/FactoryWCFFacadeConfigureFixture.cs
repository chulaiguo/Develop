using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFFacadeConfigureFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.FacadeConfigure", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFFacadeConfigure detail = new FactoryWCFFacadeConfigure(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Facade.config", this.SrcDirectory);

            FactoryWCFFacadeConfigure entity = new FactoryWCFFacadeConfigure(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}