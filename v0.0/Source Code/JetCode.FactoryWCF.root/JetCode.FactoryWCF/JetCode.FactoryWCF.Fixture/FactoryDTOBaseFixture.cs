using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOBaseFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.DTOBase", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOBase detail = new FactoryDTOBase(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DTOBase.cs", this.SrcDirectory);

            FactoryDTOBase entity = new FactoryDTOBase(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}