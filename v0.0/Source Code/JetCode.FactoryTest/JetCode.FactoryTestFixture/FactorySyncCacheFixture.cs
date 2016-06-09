using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncCacheFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncCache", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncCache detail = new FactorySyncCache(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SyncCache.cs", this.SrcDirectory);
            FactorySyncCache detail = new FactorySyncCache(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
