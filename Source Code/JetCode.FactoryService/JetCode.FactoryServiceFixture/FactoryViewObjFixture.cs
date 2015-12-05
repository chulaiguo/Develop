using JetCode.FactoryFixture;
using JetCode.FactoryService;
using NUnit.Framework;

namespace JetCode.FactoryServiceFixture
{
    [TestFixture]
    public class FactoryViewObjFixture : FixtureBase
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
            get { return string.Format(@"{0}\{1}Service.ViewMethods", BasePath, Utils._ServiceName); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryViewObj detail = new FactoryViewObj(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Biz{1}.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryViewObj detail = new FactoryViewObj(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
