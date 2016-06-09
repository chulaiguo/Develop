using JetCode.FactoryFixture;
using JetCode.FactoryWebAPI;
using NUnit.Framework;

namespace JetCode.FactoryWebAPIFixture
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
            get { return string.Format(@"{0}\Publish_WebAPI_JSON.FacadeServiceController", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeService detail = new FactoryFacadeService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\JsonFacadeServiceController.cs", this.SrcDirectory);
            FactoryFacadeService detail = new FactoryFacadeService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
