using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.DTO", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTO detail = new FactoryDTO(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DTO.cs", this.SrcDirectory);

            FactoryDTO entity = new FactoryDTO(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}