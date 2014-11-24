using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOViewFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.View", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOView detail = new FactoryDTOView(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewDTO.cs", this.SrcDirectory);

            FactoryDTOView entity = new FactoryDTOView(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}