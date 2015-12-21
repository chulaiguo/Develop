using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicGatewayService_RESTFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.BasicGatewayService_REST", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicGatewayService_REST detail = new FactoryBasicGatewayService_REST(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicServiceController.cs", this.SrcDirectory);
            FactoryBasicGatewayService_REST detail = new FactoryBasicGatewayService_REST(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
