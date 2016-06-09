using JetCode.FactoryFacadeService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryFacadeServiceFixture
{
    [TestFixture]
    public class FactoryFacadeGatewayService_RESTFixture : FixtureBase
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
            get { return string.Format(@"{0}\FacadeService.FacadeGatewayService_REST", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeGatewayService_REST detail = new FactoryFacadeGatewayService_REST(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeServiceController.cs", this.SrcDirectory);
            FactoryFacadeGatewayService_REST detail = new FactoryFacadeGatewayService_REST(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
