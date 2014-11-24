using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.TestCollection", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryCollection detail = new FactoryCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Collection.cs", this.SrcDirectory);
            FactoryCollection detail = new FactoryCollection(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
