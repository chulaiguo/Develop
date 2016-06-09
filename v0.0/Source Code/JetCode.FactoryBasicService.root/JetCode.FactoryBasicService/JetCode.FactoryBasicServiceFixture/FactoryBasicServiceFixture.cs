using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.BasicServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicService.FactoryBasicService detail = new FactoryBasicService.FactoryBasicService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicServiceFactory.cs", this.SrcDirectory);
            FactoryBasicService.FactoryBasicService detail = new FactoryBasicService.FactoryBasicService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
