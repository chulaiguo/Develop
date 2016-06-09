using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryDataServiceFactoryFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.DataServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataServiceFactory detail = new FactoryDataServiceFactory(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataServiceFactory.cs", this.SrcDirectory);
            FactoryDataServiceFactory detail = new FactoryDataServiceFactory(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
