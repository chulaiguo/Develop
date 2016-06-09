using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.IBasicServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicServiceInterface detail = new FactoryBasicServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IBasicServiceFactory.cs", this.SrcDirectory);
            FactoryBasicServiceInterface detail = new FactoryBasicServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
