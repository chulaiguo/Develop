using JetCode.FactoryFixture;
using JetCode.FactoryRemoting;
using NUnit.Framework;

namespace JetCode.FactoryRemotingFixture
{
    [TestFixture]
    public class FactoryBizServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_Remoting.I{1}ServiceFactory", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBizServiceInterface detail = new FactoryBizServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\I{1}ServiceFactory.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryBizServiceInterface detail = new FactoryBizServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
