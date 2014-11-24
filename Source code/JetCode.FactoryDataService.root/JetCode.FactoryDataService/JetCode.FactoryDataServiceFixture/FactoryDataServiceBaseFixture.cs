using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryDataServiceBaseFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.DataServiceBaseCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataServiceBase detail = new FactoryDataServiceBase(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataServiceBase.cs", this.SrcDirectory);
            FactoryDataServiceBase detail = new FactoryDataServiceBase(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
