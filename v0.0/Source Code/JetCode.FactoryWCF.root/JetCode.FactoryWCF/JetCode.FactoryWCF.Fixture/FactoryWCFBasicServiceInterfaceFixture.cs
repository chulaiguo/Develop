using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFBasicServiceInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.BasicServiceInterface", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFBasicServiceInterface detail = new FactoryWCFBasicServiceInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IBasicService.cs", this.SrcDirectory);

            FactoryWCFBasicServiceInterface entity = new FactoryWCFBasicServiceInterface(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}