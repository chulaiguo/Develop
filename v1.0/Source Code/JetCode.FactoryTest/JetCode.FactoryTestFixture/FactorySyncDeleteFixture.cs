using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncDeleteFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncDelete", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncDelete detail = new FactorySyncDelete(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SyncDelete.cs", this.SrcDirectory);
            FactorySyncDelete detail = new FactorySyncDelete(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
