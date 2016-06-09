using JetCode.FactoryDataService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataServiceFixture
{
    [TestFixture]
    public class FactoryInactiveFilterFixture : FixtureBase
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
            get
            {
                return string.Format(@"{0}\DataService.InactiveFilter", BasePath);
            }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryInactiveFilter detail = new FactoryInactiveFilter(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\InactiveFilter.cs", this.SrcDirectory);

            FactoryInactiveFilter detail = new FactoryInactiveFilter(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}