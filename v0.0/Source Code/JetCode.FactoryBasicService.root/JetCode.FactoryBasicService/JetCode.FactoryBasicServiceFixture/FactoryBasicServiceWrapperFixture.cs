using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.BasicServiceWrapper", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicServiceWrapper detail = new FactoryBasicServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicServiceWrapper.cs", this.SrcDirectory);
            FactoryBasicServiceWrapper detail = new FactoryBasicServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
