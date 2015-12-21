using JetCode.FactoryFixture;
using JetCode.FactoryService;
using NUnit.Framework;

namespace JetCode.FactoryServiceFixture
{
    [TestFixture]
    public class FactoryGatewayService_RESTFixture : FixtureBase
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
            get { return string.Format(@"{0}\{1}Service.{1}GatewayService_REST", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGatewayService_REST detail = new FactoryGatewayService_REST(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\{1}GatewayService.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryGatewayService_REST detail = new FactoryGatewayService_REST(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
