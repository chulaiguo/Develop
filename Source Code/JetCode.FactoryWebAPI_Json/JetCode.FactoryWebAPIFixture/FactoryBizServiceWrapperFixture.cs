using JetCode.FactoryFixture;
using JetCode.FactoryWebAPI;
using NUnit.Framework;

namespace JetCode.FactoryWebAPIFixture
{
    [TestFixture]
    public class FactoryBizServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_WebAPI_JSON.{1}ServiceWrapper", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBizServiceWrapper detail = new FactoryBizServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Json{1}ServiceWrapper.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryBizServiceWrapper detail = new FactoryBizServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
