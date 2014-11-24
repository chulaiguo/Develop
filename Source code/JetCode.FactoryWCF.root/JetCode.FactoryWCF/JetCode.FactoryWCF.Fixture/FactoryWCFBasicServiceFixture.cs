using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFBasicServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.BasicService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFBasicService detail = new FactoryWCFBasicService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicService.cs", this.SrcDirectory);

            FactoryWCFBasicService entity = new FactoryWCFBasicService(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}