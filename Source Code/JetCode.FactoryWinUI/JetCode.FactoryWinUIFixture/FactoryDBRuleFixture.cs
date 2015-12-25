using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
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
            get { return string.Format(@"{0}\DBRuleCode", BasePath); }
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

            string viewFileName = string.Format(@"{0}\DBRule.cs", this.SrcDirectory);

            FactoryDBRule view = new FactoryDBRule(base.Schema);
            base.WriteToFile(viewFileName, view);
        }
    }
}
