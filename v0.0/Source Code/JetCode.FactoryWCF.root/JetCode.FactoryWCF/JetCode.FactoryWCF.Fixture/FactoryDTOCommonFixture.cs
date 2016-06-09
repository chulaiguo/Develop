using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOCommonFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.Common", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOCommon detail = new FactoryDTOCommon(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DTOCommon.cs", this.SrcDirectory);

            FactoryDTOCommon entity = new FactoryDTOCommon(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}