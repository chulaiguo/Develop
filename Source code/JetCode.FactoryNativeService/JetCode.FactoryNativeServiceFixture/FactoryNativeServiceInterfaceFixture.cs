using JetCode.FactoryNativeService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryNativeServiceFixture
{
    [TestFixture]
    public class FactoryNativeServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\NativeService.INativeServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryNativeServiceInterface detail = new FactoryNativeServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\INativeServiceFactory.cs", this.SrcDirectory);
            FactoryNativeServiceInterface detail = new FactoryNativeServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
