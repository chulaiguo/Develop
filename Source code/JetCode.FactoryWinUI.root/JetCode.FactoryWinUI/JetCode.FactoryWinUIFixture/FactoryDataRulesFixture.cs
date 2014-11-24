using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryDataRulesFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules_DataRule", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataRules detail = new FactoryDataRules(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataRules.cs", this.SrcDirectory);

            FactoryDataRules detail = new FactoryDataRules(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
