using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryRules_DataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules.Data", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryRules_Data detail = new FactoryRules_Data(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\Data.cs", this.SrcDirectory);

            FactoryRules_Data detail = new FactoryRules_Data(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
