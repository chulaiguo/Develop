using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySyncInsertFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SyncInsert", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySyncInsert detail = new FactorySyncInsert(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SyncInsert.cs", this.SrcDirectory);
            FactorySyncInsert detail = new FactorySyncInsert(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
