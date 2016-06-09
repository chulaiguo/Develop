using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryDTOBizFixture : FixtureBase
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
            get { return string.Format(@"{0}\DTO.Biz", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOBiz detail = new FactoryDTOBiz(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BizDTO.cs", this.SrcDirectory);

            FactoryDTOBiz entity = new FactoryDTOBiz(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}