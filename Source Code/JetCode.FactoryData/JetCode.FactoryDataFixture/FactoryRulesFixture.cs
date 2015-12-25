using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryRulesFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_Rules", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryRules detail = new FactoryRules(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\Rules.cs", this.SrcDirectory);

            FactoryRules detail = new FactoryRules(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
