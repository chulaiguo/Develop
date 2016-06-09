using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFFacadeServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.IFacadeService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFFacadeServiceInterface detail = new FactoryWCFFacadeServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IFacadeService.cs", this.SrcDirectory);

            FactoryWCFFacadeServiceInterface entity = new FactoryWCFFacadeServiceInterface(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}