using JetCode.FactoryFixture;
using JetCode.FactoryLocalWrapper;
using NUnit.Framework;

namespace JetCode.FactoryLocalWrapperFixture
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
            get { return string.Format(@"{0}\LocalWrapper.{1}ServiceWrapper", BasePath, Utils._ServiceName); }
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

            string fileName = string.Format(@"{0}\{1}ServiceWrapper.cs", this.SrcDirectory, Utils._ServiceName);
            FactoryBizServiceWrapper detail = new FactoryBizServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
