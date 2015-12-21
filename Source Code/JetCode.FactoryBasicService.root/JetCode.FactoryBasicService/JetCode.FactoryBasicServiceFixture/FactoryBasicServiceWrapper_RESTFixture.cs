using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicServiceWrapper_RESTFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.BasicServiceWrapper_REST", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicServiceWrapper_REST detail = new FactoryBasicServiceWrapper_REST(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicServiceWrapper.cs", this.SrcDirectory);
            FactoryBasicServiceWrapper_REST detail = new FactoryBasicServiceWrapper_REST(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
