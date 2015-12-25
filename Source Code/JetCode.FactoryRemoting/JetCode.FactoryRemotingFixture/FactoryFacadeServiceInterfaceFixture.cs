using JetCode.FactoryFixture;
using JetCode.FactoryRemoting;
using NUnit.Framework;

namespace JetCode.FactoryRemotingFixture
{
    [TestFixture]
    public class FactoryFacadeServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_Remoting.IFacadeServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeServiceInterface detail = new FactoryFacadeServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IFacadeServiceFactory.cs", this.SrcDirectory);
            FactoryFacadeServiceInterface detail = new FactoryFacadeServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
