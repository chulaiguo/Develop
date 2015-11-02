using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryOnpremisesServiceFixture
{
    [TestFixture]
    public class FactoryOnpremisesServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\OnpremisesService.OnpremisesServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryOnpremisesService.FactoryOnpremisesService detail = new FactoryOnpremisesService.FactoryOnpremisesService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\OnpremisesServiceFactory.cs", this.SrcDirectory);
            FactoryOnpremisesService.FactoryOnpremisesService detail = new FactoryOnpremisesService.FactoryOnpremisesService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
