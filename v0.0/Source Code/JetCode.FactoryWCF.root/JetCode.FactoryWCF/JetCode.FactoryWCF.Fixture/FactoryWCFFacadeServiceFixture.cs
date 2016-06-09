using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFFacadeServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.FacadeService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFFacadeService detail = new FactoryWCFFacadeService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeService.cs", this.SrcDirectory);

            FactoryWCFFacadeService entity = new FactoryWCFFacadeService(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}