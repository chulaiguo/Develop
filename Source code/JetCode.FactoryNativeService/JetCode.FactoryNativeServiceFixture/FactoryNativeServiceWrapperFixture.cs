using JetCode.FactoryNativeService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryNativeServiceFixture
{
    [TestFixture]
    public class FactoryNativeServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\NativeService.NativeServiceWrapper", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryNativeServiceWrapper detail = new FactoryNativeServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\NativeServiceWrapper.cs", this.SrcDirectory);
            FactoryNativeServiceWrapper detail = new FactoryNativeServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
