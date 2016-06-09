using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryTestDataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.TestDataCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryTestData detail = new FactoryTestData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Program.cs", this.SrcDirectory);
            FactoryTestData detail = new FactoryTestData(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
