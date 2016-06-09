using JetCode.FactoryFixture;
using JetCode.FactoryWebAPI;
using NUnit.Framework;

namespace JetCode.FactoryWebAPIFixture
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
            get { return string.Format(@"{0}\Publish_WebAPI.FacadeServiceWrapper", BasePath); }
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
