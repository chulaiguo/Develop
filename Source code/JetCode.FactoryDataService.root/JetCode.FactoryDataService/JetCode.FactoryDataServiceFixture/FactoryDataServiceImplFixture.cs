using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryDataServiceImplFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.DataService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataServiceImpl detail = new FactoryDataServiceImpl(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataService.cs", this.SrcDirectory);
            FactoryDataServiceImpl detail = new FactoryDataServiceImpl(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
