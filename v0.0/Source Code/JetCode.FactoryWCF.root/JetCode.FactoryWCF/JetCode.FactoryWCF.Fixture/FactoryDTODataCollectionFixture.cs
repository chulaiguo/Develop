using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTODataCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.DataCollection", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTODataCollection detail = new FactoryDTODataCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataDTOCollection.cs", this.SrcDirectory);

            FactoryDTODataCollection entity = new FactoryDTODataCollection(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}