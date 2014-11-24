using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryRuleCheckFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules.CheckRule", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryRulesCheck detail = new FactoryRulesCheck(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\CheckRules.cs", this.SrcDirectory);

            FactoryRulesCheck detail = new FactoryRulesCheck(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
