using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryDataWithIRuleFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules_Data", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataWithIRule detail = new FactoryDataWithIRule(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataWithIRule.cs", this.SrcDirectory);

            FactoryDataWithIRule detail = new FactoryDataWithIRule(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
