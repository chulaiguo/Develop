using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryHelperCacheFixture : FixtureBase
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
            get { return string.Format(@"{0}\Helper.HelperCache", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryHelperCache detail = new FactoryHelperCache(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\HelperCache.cs", this.SrcDirectory);

            FactoryHelperCache detail = new FactoryHelperCache(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
