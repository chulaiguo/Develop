using JetCode.FactoryOnpremisesService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryOnpremisesServiceFixture
{
    [TestFixture]
    public class FactoryOnpremisesServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\OnpremisesService.OnpremisesServiceWrapper", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryOnpremisesServiceWrapper detail = new FactoryOnpremisesServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\OnpremisesServiceWrapper.cs", this.SrcDirectory);
            FactoryOnpremisesServiceWrapper detail = new FactoryOnpremisesServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
