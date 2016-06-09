using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryLinkDBFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.LinkDB", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryLinkDB detail = new FactoryLinkDB(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();


            string fileName = string.Format(@"{0}\DBLinkAction.cs", this.SrcDirectory);

            FactoryLinkDB detail = new FactoryLinkDB(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
