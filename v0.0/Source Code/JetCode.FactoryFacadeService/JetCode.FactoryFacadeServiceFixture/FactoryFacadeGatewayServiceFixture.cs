using JetCode.FactoryFacadeService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryFacadeServiceFixture
{
    [TestFixture]
    public class FactoryFacadeGatewayServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\FacadeService.FacadeGatewayService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeGatewayService detail = new FactoryFacadeGatewayService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeGatewayService.cs", this.SrcDirectory);
            FactoryFacadeGatewayService detail = new FactoryFacadeGatewayService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
