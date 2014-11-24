using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicGatewayServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.BasicGatewayService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicGatewayService detail = new FactoryBasicGatewayService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicGatewayService.cs", this.SrcDirectory);
            FactoryBasicGatewayService detail = new FactoryBasicGatewayService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
