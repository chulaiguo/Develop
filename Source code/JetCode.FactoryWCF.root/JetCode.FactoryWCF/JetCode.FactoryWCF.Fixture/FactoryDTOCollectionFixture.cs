using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.DTOCollection", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOCollection detail = new FactoryDTOCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DTOCollection.cs", this.SrcDirectory);

            FactoryDTOCollection entity = new FactoryDTOCollection(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}