using JetCode.FactoryFixture;
using JetCode.FactoryService;
using NUnit.Framework;

namespace JetCode.FactoryServiceFixture
{
    [TestFixture]
    public class FactoryGatewayServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\{1}Service.{1}GatewayService", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGatewayService detail = new FactoryGatewayService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\{1}GatewayService.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryGatewayService detail = new FactoryGatewayService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
