using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryIRulesDataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules_Interface", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryIRulesData detail = new FactoryIRulesData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\IRulesData.cs", this.SrcDirectory);

            FactoryIRulesData detail = new FactoryIRulesData(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
