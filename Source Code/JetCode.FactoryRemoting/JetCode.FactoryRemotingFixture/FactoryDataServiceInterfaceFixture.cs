using JetCode.FactoryFixture;
using JetCode.FactoryRemoting;
using NUnit.Framework;

namespace JetCode.FactoryRemotingFixture
{
    [TestFixture]
    public class FactoryDataServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_Remoting.IDataServiceFactory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataServiceInterface detail = new FactoryDataServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IDataServiceFactory.cs", this.SrcDirectory);
            FactoryDataServiceInterface detail = new FactoryDataServiceInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
