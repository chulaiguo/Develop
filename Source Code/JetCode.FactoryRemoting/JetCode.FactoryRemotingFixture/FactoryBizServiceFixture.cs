using JetCode.FactoryFixture;
using JetCode.FactoryRemoting;
using NUnit.Framework;

namespace JetCode.FactoryRemotingFixture
{
    [TestFixture]
    public class FactoryBizServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_Remoting.{1}ServiceFactory", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBizService detail = new FactoryBizService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\{1}ServiceFactory.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryBizService detail = new FactoryBizService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
