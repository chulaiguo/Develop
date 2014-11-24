using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryRule_CheckRuleFixture : FixtureBase
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
            FactoryRules_CheckRule detail = new FactoryRules_CheckRule(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\CheckRules.cs", this.SrcDirectory);

            FactoryRules_CheckRule detail = new FactoryRules_CheckRule(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
