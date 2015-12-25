using JetCode.FactoryFixture;
using JetCode.FactoryLocalWrapper;
using NUnit.Framework;

namespace JetCode.FactoryLocalWrapperFixture
{
    [TestFixture]
    public class FactoryDataServiceWrapperFixture : FixtureBase
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
            get { return string.Format(@"{0}\LocalWrapper.DataServiceWrapper", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataServiceWrapper detail = new FactoryDataServiceWrapper(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataServiceWrapper.cs", this.SrcDirectory);
            FactoryDataServiceWrapper detail = new FactoryDataServiceWrapper(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
