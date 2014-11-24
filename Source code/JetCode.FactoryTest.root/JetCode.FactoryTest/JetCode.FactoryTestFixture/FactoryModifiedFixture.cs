using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryModifiedFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.Modified", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryModified detail = new FactoryModified(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Modified.cs", this.SrcDirectory);
            FactoryModified detail = new FactoryModified(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
