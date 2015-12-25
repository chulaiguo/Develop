using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncUpdateFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncUpdate", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncUpdate detail = new FactorySyncUpdate(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SyncUpdate.cs", this.SrcDirectory);
            FactorySyncUpdate detail = new FactorySyncUpdate(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
