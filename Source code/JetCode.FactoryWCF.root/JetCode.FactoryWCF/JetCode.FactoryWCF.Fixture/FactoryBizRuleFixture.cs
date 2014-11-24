using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryBizRuleFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.BizRule", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBizRule detail = new FactoryBizRule(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BizRule.cs", this.SrcDirectory);

            FactoryBizRule entity = new FactoryBizRule(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}