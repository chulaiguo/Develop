using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryRules_DataRuleFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules.DataRule", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryRules_DataRule detail = new FactoryRules_DataRule(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataRules.cs", this.SrcDirectory);

            FactoryRules_DataRule detail = new FactoryRules_DataRule(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
