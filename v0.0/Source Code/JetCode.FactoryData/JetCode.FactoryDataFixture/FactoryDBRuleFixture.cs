using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryDBRuleFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_DBRule", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDBRule detail = new FactoryDBRule(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DBRuleConstant.cs", this.SrcDirectory);

            FactoryDBRule detail = new FactoryDBRule(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
