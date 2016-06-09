using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryFacadeServiceFixture
{
    [TestFixture]
    public class FactoryFacadeServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\FacadeService.FacadeServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeService.FactoryFacadeService detail = new FactoryFacadeService.FactoryFacadeService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeServiceFactory.cs", this.SrcDirectory);
            FactoryFacadeService.FactoryFacadeService detail = new FactoryFacadeService.FactoryFacadeService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
