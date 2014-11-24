using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryHelperSyncDataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Helper.HelperSync", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryHelperSyncData detail = new FactoryHelperSyncData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\HelperSync.cs", this.SrcDirectory);
            FactoryHelperSyncData detail = new FactoryHelperSyncData(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
