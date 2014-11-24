using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryUpdateCacheFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.UpdateCache", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryUpdateCache detail = new FactoryUpdateCache(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\UpdateCache.cs", this.SrcDirectory);

            FactoryUpdateCache detail = new FactoryUpdateCache(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
