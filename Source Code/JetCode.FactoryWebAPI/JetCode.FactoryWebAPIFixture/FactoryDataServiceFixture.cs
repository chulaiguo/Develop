using JetCode.FactoryFixture;
using JetCode.FactoryWebAPI;
using NUnit.Framework;

namespace JetCode.FactoryWebAPIFixture
{
    [TestFixture]
    public class FactoryDataServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_WebAPI.DataServiceController", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataService detail = new FactoryDataService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataServiceController.cs", this.SrcDirectory);
            FactoryDataService detail = new FactoryDataService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
