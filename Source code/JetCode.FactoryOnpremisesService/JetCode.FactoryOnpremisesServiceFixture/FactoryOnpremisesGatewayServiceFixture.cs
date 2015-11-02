using JetCode.FactoryOnpremisesService;
using JetCode.FactoryOnpremisesService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryOnpremisesServiceFixture
{
    [TestFixture]
    public class FactoryOnpremisesGatewayServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\OnpremisesService.OnpremisesGatewayService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryOnpremisesGatewayService detail = new FactoryOnpremisesGatewayService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\OnpremisesGatewayService.cs", this.SrcDirectory);
            FactoryOnpremisesGatewayService detail = new FactoryOnpremisesGatewayService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
