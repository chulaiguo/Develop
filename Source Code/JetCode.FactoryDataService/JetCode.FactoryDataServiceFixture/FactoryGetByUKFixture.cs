using JetCode.FactoryFixture;
using JetCode.FactoryDataService;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryGetByUKFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByUK", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByUK detail = new FactoryGetByUK(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByUK.cs", this.SrcDirectory);
            FactoryGetByUK detail = new FactoryGetByUK(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
