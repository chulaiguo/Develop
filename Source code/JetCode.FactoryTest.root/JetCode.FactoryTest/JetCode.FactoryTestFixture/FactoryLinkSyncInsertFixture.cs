using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryLinkSyncInsertFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.LinkInsert", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryLinkSyncInsert detail = new FactoryLinkSyncInsert(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Insert.cs", this.SrcDirectory);
            FactoryLinkSyncInsert detail = new FactoryLinkSyncInsert(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
