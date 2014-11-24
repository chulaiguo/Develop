using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryGetByIndexFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByIndexCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByIndex detail = new FactoryGetByIndex(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByIndex.cs", this.SrcDirectory);
            FactoryGetByIndex detail = new FactoryGetByIndex(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
