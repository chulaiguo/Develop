using JetCode.FactoryOnpremisesService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryOnpremisesServiceFixture
{
    [TestFixture]
    public class FactoryOnpremisesServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\OnpremisesService.IOnpremisesServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryOnpremisesServiceInterface detail = new FactoryOnpremisesServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IOnpremisesServiceFactory.cs", this.SrcDirectory);
            FactoryOnpremisesServiceInterface detail = new FactoryOnpremisesServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
