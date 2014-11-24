using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTODataFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.Data", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOData detail = new FactoryDTOData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataDTO.cs", this.SrcDirectory);

            FactoryDTOData entity = new FactoryDTOData(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}