using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryRules_TooltipFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules.Tooltip", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
                FactoryRules_Tooltip_Biz toolTipBiz = new FactoryRules_Tooltip_Biz(base.Schema);
                base.WriteToScreen(toolTipBiz);

                FactoryRules_Tooltip_DB tooltipDb = new FactoryRules_Tooltip_DB(base.Schema);
                base.WriteToScreen(tooltipDb);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string dbFileName = string.Format(@"{0}\DBTooltips.cs", this.SrcDirectory);
            string bizFileName = string.Format(@"{0}\BizTooltips.cs", this.SrcDirectory);

            FactoryRules_Tooltip_Biz toolTipBiz = new FactoryRules_Tooltip_Biz(base.Schema);
            base.WriteToFile(bizFileName, toolTipBiz);

            FactoryRules_Tooltip_DB tooltipDb = new FactoryRules_Tooltip_DB(base.Schema);
            base.WriteToFile(dbFileName, tooltipDb);
        }
    }
}
