using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncBaseFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncBase", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncBase detail = new FactorySyncBase(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SyncBase.cs", this.SrcDirectory);
            FactorySyncBase detail = new FactorySyncBase(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
