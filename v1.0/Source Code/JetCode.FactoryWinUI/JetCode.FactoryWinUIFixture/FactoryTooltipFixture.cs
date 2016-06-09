using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryTooltipFixture : FixtureBase
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
            get { return string.Format(@"{0}\TooltipCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryTooltip detail = new FactoryTooltip(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string viewFileName = string.Format(@"{0}\DBTooltips.cs", this.SrcDirectory);

            FactoryTooltip view = new FactoryTooltip(base.Schema);
            base.WriteToFile(viewFileName, view);
        }
    }
}
