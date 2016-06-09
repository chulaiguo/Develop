using JetCode.FactoryFixture;
using JetCode.FactoryService;
using NUnit.Framework;

namespace JetCode.FactoryServiceFixture
{
    [TestFixture]
    public class FactoryServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\{1}Service.I{1}ServiceFactory", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryServiceInterface detail = new FactoryServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\I{1}ServiceFactory.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryServiceInterface detail = new FactoryServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
