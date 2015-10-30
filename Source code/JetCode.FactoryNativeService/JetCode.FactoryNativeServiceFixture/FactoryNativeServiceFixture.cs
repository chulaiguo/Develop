using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryNativeServiceFixture
{
    [TestFixture]
    public class FactoryNativeServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\NativeService.NativeServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryNativeService.FactoryNativeService detail = new FactoryNativeService.FactoryNativeService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\NativeServiceFactory.cs", this.SrcDirectory);
            FactoryNativeService.FactoryNativeService detail = new FactoryNativeService.FactoryNativeService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
