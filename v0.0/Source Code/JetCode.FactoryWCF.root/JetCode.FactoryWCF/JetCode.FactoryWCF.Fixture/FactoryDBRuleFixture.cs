using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
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
            get { return string.Format(@"{0}\WCF.DBRule", BasePath); }
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

            string fileName = string.Format(@"{0}\DBRule.cs", this.SrcDirectory);

            FactoryDBRule entity = new FactoryDBRule(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}