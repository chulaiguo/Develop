using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncClearFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncClearDB", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncClear detail = new FactorySyncClear(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ClearDB.cs", this.SrcDirectory);
            FactorySyncClear detail = new FactorySyncClear(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
