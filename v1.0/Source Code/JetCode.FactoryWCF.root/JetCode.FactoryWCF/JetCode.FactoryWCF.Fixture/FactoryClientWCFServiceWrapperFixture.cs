using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryClientWCFServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.ClientWCFServiceWrapper", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryClientWCFServiceWrapper detail = new FactoryClientWCFServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\WCFServiceWrapper.cs", this.SrcDirectory);

            FactoryClientWCFServiceWrapper entity = new FactoryClientWCFServiceWrapper(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}