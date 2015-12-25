using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
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
            get { return string.Format(@"{0}\Misc.DBRule", BasePath); }
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
            FactoryDBRule detail = new FactoryDBRule(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
