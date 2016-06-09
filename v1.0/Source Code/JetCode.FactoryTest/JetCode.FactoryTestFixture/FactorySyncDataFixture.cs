using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncDataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncData", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncData detail = new FactorySyncData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SyncData.cs", this.SrcDirectory);
            FactorySyncData detail = new FactorySyncData(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
