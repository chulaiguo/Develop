using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryTestFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.TestCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryTest detail = new FactoryTest(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Modified.cs", this.SrcDirectory);
            FactoryTest detail = new FactoryTest(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
