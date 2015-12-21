using JetCode.FactoryFixture;
using JetCode.FactoryService;
using NUnit.Framework;

namespace JetCode.FactoryServiceFixture
{
    [TestFixture]
    public class FactoryServiceWrapper_RESTFixture : FixtureBase
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
            get { return string.Format(@"{0}\{1}Service.{1}ServiceWrapper_REST", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryServiceWrapper_REST detail = new FactoryServiceWrapper_REST(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\{1}ServiceWrapper.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryServiceWrapper_REST detail = new FactoryServiceWrapper_REST(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
