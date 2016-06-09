using JetCode.FactoryFacadeService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryFacadeServiceFixture
{
    [TestFixture]
    public class FactoryFacadeServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\FacadeService.FacadeServiceWrapper", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeServiceWrapper detail = new FactoryFacadeServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeServiceWrapper.cs", this.SrcDirectory);
            FactoryFacadeServiceWrapper detail = new FactoryFacadeServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
