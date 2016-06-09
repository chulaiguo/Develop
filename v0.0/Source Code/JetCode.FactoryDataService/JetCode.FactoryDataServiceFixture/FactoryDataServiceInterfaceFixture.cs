using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryDataServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.IDataService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataServiceInterface detail = new FactoryDataServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IDataService.cs", this.SrcDirectory);
            FactoryDataServiceInterface detail = new FactoryDataServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
