using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryCRUDFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.CRUDCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryCRUD detail = new FactoryCRUD(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\CRUD.cs", this.SrcDirectory);
            FactoryCRUD detail = new FactoryCRUD(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
