using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryCRUDLiteFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.CRUDLite", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryCRUDLite detail = new FactoryCRUDLite(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DAL.cs", this.SrcDirectory);
            FactoryCRUDLite detail = new FactoryCRUDLite(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
