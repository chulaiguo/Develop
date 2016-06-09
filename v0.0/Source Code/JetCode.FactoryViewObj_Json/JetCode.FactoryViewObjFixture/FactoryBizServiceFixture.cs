using JetCode.FactoryFixture;
using JetCode.FactoryViewObj;
using NUnit.Framework;

namespace JetCode.FactoryViewObjFixture
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
            get { return string.Format(@"{0}\ViewObj_JSON.{1}Methods", BasePath, Utils._ServiceName); }
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

            string fileName = string.Format(@"{0}\{1}Methods.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryBizService detail = new FactoryBizService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
