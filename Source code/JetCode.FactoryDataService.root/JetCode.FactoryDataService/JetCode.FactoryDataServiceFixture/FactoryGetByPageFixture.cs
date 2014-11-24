using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryGetByPageFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByPageCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByPage detail = new FactoryGetByPage(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByPage.cs", this.SrcDirectory);
            FactoryGetByPage detail = new FactoryGetByPage(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
