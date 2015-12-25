using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryFacadeServiceFixture
{
    [TestFixture]
    public class FactoryFacadeServiceViewObjFixture : FixtureBase
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
            get { return string.Format(@"{0}\FacadeService.ViewMethods", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeService.FactoryFacadeServiceViewObj detail = new FactoryFacadeService.FactoryFacadeServiceViewObj(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeMethods.cs", this.SrcDirectory);
            FactoryFacadeService.FactoryFacadeServiceViewObj detail = new FactoryFacadeService.FactoryFacadeServiceViewObj(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
