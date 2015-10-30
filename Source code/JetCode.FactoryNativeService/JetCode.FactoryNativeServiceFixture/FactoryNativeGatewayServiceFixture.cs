using JetCode.FactoryNativeService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryNativeServiceFixture
{
    [TestFixture]
    public class FactoryNativeGatewayServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\NativeService.NativeGatewayService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryNativeGatewayService detail = new FactoryNativeGatewayService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\NativeGatewayService.cs", this.SrcDirectory);
            FactoryNativeGatewayService detail = new FactoryNativeGatewayService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
