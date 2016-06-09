using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOBizCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.BizCollection", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOBizCollection detail = new FactoryDTOBizCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BizDTOCollection.cs", this.SrcDirectory);

            FactoryDTOBizCollection entity = new FactoryDTOBizCollection(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}