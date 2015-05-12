using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryGetByMapFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByMap", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByMap detail = new FactoryGetByMap(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByMapped.cs", this.SrcDirectory);
            FactoryGetByMap detail = new FactoryGetByMap(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
