using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOViewCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.ViewCollection", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOViewCollection detail = new FactoryDTOViewCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewDTOCollection.cs", this.SrcDirectory);

            FactoryDTOViewCollection entity = new FactoryDTOViewCollection(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}